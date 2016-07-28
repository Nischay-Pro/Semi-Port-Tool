Imports System.IO
Imports System.IO.Compression
Imports Newtonsoft.Json.Linq
Public Class toolsgui
    Dim jsondata As String
    Private Sub toolsgui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Me.AllowDrop = True
        jsondata = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\tools\data.json")
        Dim Data = JObject.Parse(jsondata)
        Dim i As Integer
        Do Until i = Data("tools").Count
            ComboBox1.Items.Add(Data("tools")(i)("goodname"))
            i += 1
        Loop
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox1.SelectedItem.ToString = "Diff Tool Creator" Then
            creatediff.Show()
        ElseIf ComboBox1.SelectedItem.ToString = "Diff Tool Versioning" Then
            tools.Show()
        Else
            If launchpath <> "" And runas = False Then
                Process.Start(launchpath)
            Else
                Dim procStartInfo As New ProcessStartInfo
                Dim procExecuting As New Process

                With procStartInfo
                    .UseShellExecute = True
                    .FileName = launchpath
                    .WindowStyle = ProcessWindowStyle.Normal
                    .Verb = "runas"
                End With

                procExecuting = Process.Start(procStartInfo)
            End If
        End If
    End Sub
    Dim launchpath As String
    Dim runas As Boolean = False
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem.ToString = "Diff Tool Creator" Then
            Label3.Text = "Author : Nischay Pro"
            Label4.Text = "Status : Installed | Ready to launch"
            Label6.Text = "Version : " & My.Application.Info.Version.ToString
            Label5.Text = "Description : Allows you to create a JSON Configuration file containing the meta data of all your PORTed files. This allows you to be later reused when making the incremental patch."
        ElseIf ComboBox1.SelectedItem.ToString = "Diff Tool Versioning" Then
            Label3.Text = "Author : Nischay Pro"
            Label4.Text = "Status : Installed | Ready to launch"
            Label6.Text = "Version : " & My.Application.Info.Version.ToString
            Label5.Text = "Description : Using the JSON Configuration File you created allows you to generate incremental patches."
        Else
            Dim Data = JObject.Parse(jsondata)
            Dim i As Integer
            Do Until i = Data("tools").Count
                If Data("tools")(i)("goodname") = ComboBox1.SelectedItem.ToString Then
                    Label3.Text = "Author : " & Data("tools")(i)("author").ToString
                    If Data("tools")(i)("installed").ToString = "True" And Data("tools")(i)("downloaded").ToString = "True" Then
                        Label4.Text = "Status : Installed | Ready to launch"
                    ElseIf Data("tools")(i)("downloaded").ToString = "False" Then
                        Label4.Text = "Status : Need to Download | Packages Missing"
                    End If
                    If Data("tools")(i)("runas").ToString = "True" Then
                        runas = True
                    End If
                    launchpath = GetLaunchPath(Data("tools")(i))
                    Label6.Text = "Version : " & Data("tools")(i)("version").ToString
                    Label5.Text = "Description : " & Data("tools")(i)("description").ToString
                    Exit Do
                End If
                i += 1
            Loop
        End If

    End Sub

    Private Function GetLaunchPath(ByVal JOb As JObject)
        Dim temp As String
        temp = My.Application.Info.DirectoryPath & "\tools\"
        temp += JOb("launch").ToString

        If temp.Contains("%name%") Then
            temp = temp.Replace("%name%", JOb("name").ToString)
        End If
        Return temp
    End Function

    Private Sub toolsgui_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        'If e.Data.GetDataPresent(DataFormats.FileDrop) Then
        'e.Effect = DragDropEffects.All
        ' End If
    End Sub

    Private Sub toolsgui_DragLeave(sender As Object, e As EventArgs) Handles Me.DragLeave
        'Me.Cursor = Cursors.Default
    End Sub
    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        Dim openfile As New OpenFileDialog
        openfile.Filter = "Zip Package Files|*.zip"
        openfile.Title = "Select Semi Port Tool Package"
        If openfile.ShowDialog = DialogResult.OK Then
            loc = openfile.FileName
            Dim installpack As New System.Threading.Thread(AddressOf InstallPackage)
            installpack.IsBackground = True
            installpack.Start()
        End If
    End Sub
    Dim loc As String
    Private Sub InstallPackage()
        SetLabelText("Verifying Package", Label8)
        If My.Computer.FileSystem.DirectoryExists(My.Application.Info.DirectoryPath & "\temp") Then
            My.Computer.FileSystem.DeleteDirectory(My.Application.Info.DirectoryPath & "\temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(My.Application.Info.DirectoryPath & "\temp")
        Dim dest As String = My.Application.Info.DirectoryPath & "\temp\"
        ZipFile.ExtractToDirectory(loc, dest)
        If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\temp\install.json") Then
            SetLabelText("Installing Package", Label8)
            Dim install As JObject = JObject.Parse(jsondata)
            Dim installarr As JArray = install("tools")
            Dim readdata As String = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\temp\install.json")
            Dim obj As JObject = JObject.Parse(readdata)
            obj.Add("installed", "True")
            obj.Add("downloaded", "True")
            installarr.Add(obj)
            Dim main As New JObject
            main.Add("description", "Storing Tool Data")
            main.Add("tools", installarr)
            File.WriteAllText(My.Application.Info.DirectoryPath & "\tools\data.json", main.ToString)
            My.Computer.FileSystem.CopyDirectory(My.Application.Info.DirectoryPath & "\temp\", My.Application.Info.DirectoryPath & "\tools\" & obj("name").ToString)
            Kill(My.Application.Info.DirectoryPath & "\temp\install.json")
            My.Computer.FileSystem.DeleteDirectory(My.Application.Info.DirectoryPath & "\temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
            SetLabelText("Installed Package", Label8)
            updated = True
        End If
    End Sub
    Dim updated As Boolean = False
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If updated <> False Then
            Me.Close()
            Form1.ReOpen()
        End If
    End Sub
End Class