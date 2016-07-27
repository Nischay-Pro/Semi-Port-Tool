Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class tools
    Dim jsondata As String
    Dim portmain As String
    Dim destination As String
    Private Sub tools_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim openfolder As New FolderBrowserDialog
        openfolder.ShowNewFolderButton = True
        openfolder.Description = "Navigate to the folder where your PORTed ROM is."
        If openfolder.ShowDialog = DialogResult.OK Then
            If destination = openfolder.SelectedPath Then
                MsgBox("PORT folder can't be same as Destination folder", MsgBoxStyle.Exclamation, "Folder Selection Error")
                Exit Sub
            End If
            portmain = openfolder.SelectedPath
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim openfolder As New FolderBrowserDialog
        openfolder.ShowNewFolderButton = True
        openfolder.Description = "Navigate to the folder where your want to save the Differentiated PORT ROM."
        If openfolder.ShowDialog = DialogResult.OK Then
            If portmain = openfolder.SelectedPath Then
                MsgBox("Destination folder can't be same as PORT folder", MsgBoxStyle.Exclamation, "Folder Selection Error")
                Exit Sub
            End If
            destination = openfolder.SelectedPath
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openme As New OpenFileDialog
        openme.Title = "Select the file containing the PORT File Hashes."
        openme.Filter = "JSON Files|*.json"
        If openme.ShowDialog = DialogResult.OK Then
            jsondata = openme.FileName
        End If
        Button1.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim threadman As New Threading.Thread(AddressOf RunDiff)
        threadman.Start()
    End Sub

    Private Sub RunDiff()
        SetLabelText("Status : Preparing Diff Tool", Label6)
        Dim listman, listmanfiles As New ListBox
        Dim compman, compmanfiles As New ListBox
        For Each Item As String In My.Computer.FileSystem.GetFiles(portmain, FileIO.SearchOption.SearchAllSubDirectories)
            Dim temp As String = Item
            temp = temp.Replace(portmain & "\", "")
            listman.Items.Add(temp & ":" & md5_hash(Item))
            listmanfiles.Items.Add(temp)
            SetLabelText("Status : Total files detected " & listman.Items.Count, Label6)
        Next
        SetLabelText("Status : Generating HASHes for comparison", Label6)
        Dim ReadData2 As String = My.Computer.FileSystem.ReadAllText(jsondata)
        Dim Item3 = Linq.JObject.Parse("{ data:" & ReadData2 & "}")
        Dim int As Integer = 0
        Dim done As Boolean = False
        Dim temp2 As String = ""
        Do Until int = Item3("data").Children.Count
            For Each filename As JValue In Item3("data")(int).Values
                SetLabelText("Status : Loading JSON File. Loaded " & compman.Items.Count & " files", Label6)
                If done = True Then
                    compman.Items.Add(temp2 & ":" & filename.ToString)
                    compmanfiles.Items.Add(temp2)
                    int += 1
                    done = False
                Else
                    temp2 = filename.ToString
                    done = True
                End If
            Next
        Loop
        Dim result As List(Of String) = (From s1 As String In listman.Items Where Not compman.Items.Contains(s1) Select s1).ToList()
        Dim killresult As List(Of String) = (From s1 As String In compmanfiles.Items Where Not listmanfiles.Items.Contains(s1) Select s1).ToList()
        SetLabelText("Status : Found out " & result.Count & " unique modifications " & killresult.Count & " deletions", Label6)
    End Sub
End Class