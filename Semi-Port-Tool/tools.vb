Imports System.Threading
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
            If jsondata <> "" And portmain <> "" And destination <> "" Then
                Button3.Enabled = True
            End If
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
            If jsondata <> "" And portmain <> "" And destination <> "" Then
                Button3.Enabled = True
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openme As New OpenFileDialog
        openme.Title = "Select the file containing the PORT File Hashes."
        openme.Filter = "JSON Files|*.json"
        If openme.ShowDialog = DialogResult.OK Then
            jsondata = openme.FileName
            Button1.Enabled = False
        End If
        If jsondata <> "" And portmain <> "" And destination <> "" Then
            Button3.Enabled = True
        End If
    End Sub
    Dim activethreads As New ListBox
    Dim threadman As Thread
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cts As New CancellationTokenSource
        threadman = New Threading.Thread(AddressOf RunDiff)
        threadman.IsBackground = True
        activethreads.Items.Add(threadman.ManagedThreadId)
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
        Dim tempa As ListBox = listman
        For Each itm In compman.Items
            If tempa.Items.Contains(itm) Then tempa.Items.Remove(itm)
        Next
        For Each itm In listmanfiles.Items
            If compmanfiles.Items.Contains(itm) Then compmanfiles.Items.Remove(itm)
        Next
        Dim result As List(Of String) = tempa.Items.Cast(Of String).ToList
        Dim killresult As List(Of String) = compmanfiles.Items.Cast(Of String).ToList
        '
        '       Doesn't work on Mono :(
        '
        'Dim result As List(Of String) = (From s1 As String In listman.Items Where Not compman.Items.Contains(s1) Select s1).ToList()
        'Dim killresult As List(Of String) = (From s1 As String In compmanfiles.Items Where Not listmanfiles.Items.Contains(s1) Select s1).ToList()
        SetLabelText("Status : Found out " & result.Count & " unique modifications " & killresult.Count & " deletions", Label6)
        wait(1000)
        If result.Count <> 0 Then
            SetLabelText("Status : Generating Folder Structure", Label6)
            For Each File As String In result
                Dim fullpath As String = portmain & "\" & File
                Dim split As String() = fullpath.Split("\")
                Dim inta As Integer = 0
                Dim structured As String = ""
                Dim detectedsystem As Boolean = False
                For Each Item As String In split
                    inta += 1
                    If inta = split.Count Then
                        detectedsystem = False
                        structured = ""
                        inta = 0
                    Else
                        If detectedsystem = True Then
                            My.Computer.FileSystem.CreateDirectory(destination & "\" & structured & "\" & Item)
                            structured += "\" & Item
                        End If
                        If Item = "system" And detectedsystem = False Then
                            structured += "\system"
                            detectedsystem = True
                        End If
                    End If
                Next
            Next
            SetLabelText("Status : Folder Structure Generation Complete", Label6)
            wait(1000)
            SetLabelText("Status : Copying Modifications", Label6)
            For Each File1 As String In result
                Dim File As String() = File1.Split(":")
                Dim fullpathsource As String = portmain & "\" & File(0)
                Dim fullpathdestination As String = destination & "\system\" & File(0)
                FileCopy(fullpathsource, fullpathdestination)
            Next
            SetLabelText("Status : Incremental Update Folder Generated", Label6)
        Else
            SetLabelText("Status : JSON Configuration File is in sync with PORT folder.", Label6)
        End If
        activethreads.Items.Remove(Thread.CurrentThread.ManagedThreadId)
    End Sub

    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub

    Private Sub tools_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If activethreads.Items.Count <> 0 Then
            If MessageBox.Show("An active process(es) is running. Do you wish to aggressively terminate the process?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                threadman.Abort()
            Else
                e.Cancel = True
            End If
        End If
    End Sub
End Class