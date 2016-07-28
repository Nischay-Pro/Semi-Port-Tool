Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class creatediff
    Dim portmain As String
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim openfolder As New FolderBrowserDialog
        openfolder.ShowNewFolderButton = True
        openfolder.Description = "Navigate to the folder where your PORTed ROM is."
        If openfolder.ShowDialog = DialogResult.OK Then
            portmain = openfolder.SelectedPath
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim threadman As New Threading.Thread(AddressOf RunDiff)
        threadman.SetApartmentState(Threading.ApartmentState.STA)
        threadman.IsBackground = True
        threadman.Start()
    End Sub

    Private Sub RunDiff()
        SetLabelText("Status : Running Diff Tool", Label6)
        Dim savefile As New SaveFileDialog
        savefile.Title = "Select Directory to save file hashes"
        savefile.Filter = "JSON Configuration File|*.json"
        If savefile.ShowDialog = DialogResult.OK Then
            Dim jsondata As New JArray
            Dim count As Integer = 0
            For Each Item As String In My.Computer.FileSystem.GetFiles(portmain, FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                count += 1
                SetLabelText("Status : Successfully Stored Meta Data of " & count & " files", Label6)
                Dim hash_md5 = md5_hash(Item)
                Dim temp As String = Item
                temp = temp.Replace(portmain & "\", "")
                jsondata.Add(New JObject(
                             New JProperty("filename", temp),
                             New JProperty("md5", hash_md5)))
            Next
            File.WriteAllText(savefile.FileName, jsondata.ToString)
        End If
        SetLabelText("Status : Successfully generated JSON Configuration File", Label6)
    End Sub
End Class