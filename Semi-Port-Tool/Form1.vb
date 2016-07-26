Imports Newtonsoft.Json
Imports System.Threading
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text += My.Application.Info.Version.ToString
        Dim startup As New Thread(AddressOf StartupEngine)
        startup.Start()
    End Sub
    Private Sub wait(ByVal interval As Integer)
        Dim sw As New Stopwatch
        sw.Start()
        Do While sw.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        sw.Stop()
    End Sub
    Dim devicelist As Integer
    Public deviceloc As New ListBox
    Private Sub StartupEngine()
        wait(1000)
        SetLabelText("Status : Checking Directory", Label6)
        If My.Computer.FileSystem.DirectoryExists(My.Application.Info.DirectoryPath & "\devices") = False Then
            ThrowException(Nothing, "Directory Missing", ExceptionType.Critical)
            SetLabelText("Status : Shit", Label6)
            Exit Sub
        End If
        SetLabelText("Status : Generating Device Data", Label6)
        For Each Directory As String In My.Computer.FileSystem.GetDirectories(My.Application.Info.DirectoryPath & "\devices")
            Try
                Dim ReadData As String = My.Computer.FileSystem.ReadAllText(Directory & "\main.json")
                Dim Item = Linq.JObject.Parse(ReadData)
                AddItem(Item.Item("goodname").ToString, ComboBox1)
                devicelist += 1
                deviceloc.Items.Add(Directory & "\main.json!" & Item.Item("goodname").ToString)
            Catch ex As Exception
                ThrowException(ex, Nothing, ExceptionType.Warning)
                Exit Sub
            End Try
            SetLabelText("Status : Welcome. Please load PORT and BASE folders to begin working.", Label6)
        Next
    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        advanced.Show()
    End Sub
    Dim base, port As String
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim openfolder As New FolderBrowserDialog
        openfolder.ShowNewFolderButton = True
        openfolder.Description = "Navigate to the folder where your BASE rom is or the folder of the ROM you want to port."
        If openfolder.ShowDialog = DialogResult.OK Then
            If port = openfolder.SelectedPath Then
                MsgBox("BASE/STOCK folder can't be PORT folder", MsgBoxStyle.Exclamation, "Folder Selection Error")
                Exit Sub
            End If
            base = openfolder.SelectedPath
            Button1.Enabled = False
            Button2.Enabled = False
            Dim checkbase As New Thread(AddressOf VerifyBase)
            checkbase.Start()
        End If
    End Sub

    Private Sub VerifyBase()
        SetLabelText("Status : Verifying BASE Rom", Label6)
        Dim ReadData As String = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\service\directory.json")
        Dim Item = Linq.JObject.Parse(ReadData)
        For Each Item2 As String In Item.Item("scan").ToArray
            Try
                If My.Computer.FileSystem.DirectoryExists(base & "\" & Item2) = False Then
                    If My.Computer.FileSystem.FileExists(base & "\" & Item2) = False Then
                        SetLabelText("Status : Could not detect " & Item2, Label6)
                        base = Nothing
                        ToggleEnableButton(True, Button1)
                        ToggleEnableButton(True, Button2)
                        Exit Sub
                    End If
                Else
                    SetLabelText("Status : Scanning for " & Item2, Label6)
                End If
            Catch ex As Exception
                ThrowException(ex, "Directory Scan Crash", ExceptionType.Critical)
                Exit Sub
            End Try
        Next
        If port = "" Then
            SetLabelText("Status : BASE folder verified. Please load PORT folder.", Label6)
            ToggleEnableButton(True, Button1)
        Else
            SetLabelText("Status : Your good to go.", Label6)
            ShowBaseStock()
        End If
        SetLabelText("Base/Stock Loaded", Button2)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openfolder As New FolderBrowserDialog
        openfolder.ShowNewFolderButton = True
        openfolder.Description = "Navigate to the folder where your PORT rom is or the folder of the ROM compatible with your device."
        If openfolder.ShowDialog = DialogResult.OK Then
            If base = openfolder.SelectedPath Then
                MsgBox("PORT folder can't be BASE/STOCK folder", MsgBoxStyle.Exclamation, "Folder Selection Error")
                Exit Sub
            End If
            port = openfolder.SelectedPath
            Button1.Enabled = False
            Button2.Enabled = False
            Dim checkbase As New Thread(AddressOf VerifyPort)
            checkbase.Start()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Button1.Enabled = True
        Button2.Enabled = True
        CheckBox1.Enabled = True
    End Sub

    Private Sub VerifyPort()
        SetLabelText("Status : Verifying PORT Rom", Label6)
        Dim ReadData As String = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\service\directory.json")
        Dim Item = Linq.JObject.Parse(ReadData)
        For Each Item2 As String In Item.Item("scan").ToArray
            Try
                If My.Computer.FileSystem.DirectoryExists(base & "\" & Item2) = False Then
                    If My.Computer.FileSystem.FileExists(base & "\" & Item2) = False Then
                        SetLabelText("Status : Could not detect " & Item2, Label6)
                        base = Nothing
                        ToggleEnableButton(True, Button1)
                        ToggleEnableButton(True, Button2)
                        Exit Sub
                    End If
                Else
                    SetLabelText("Status : Scanning for " & Item2, Label6)
                End If
            Catch ex As Exception
                ThrowException(ex, "Directory Scan Crash", ExceptionType.Critical)
                Exit Sub
            End Try
        Next
        If port = "" Then
            SetLabelText("Status : PORT folder verified. Please load BASE folder.", Label6)
            ToggleEnableButton(True, Button2)
        Else
            SetLabelText("Status : Your good to go.", Label6)
            ToggleVisiblity(True, Button4)
        End If
        SetLabelText("Port Loaded", Button1)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Button3.Enabled = CheckBox1.Checked
        If CheckBox1.Checked = False Then
            advanced.Hide()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Button4.Enabled = False
        Button4.Text = "Porting"
        Label6.Text = "Status : Running Pre-Port Checklist"
        PrePort()
    End Sub
    Private Sub PrePort()
        wait(2000)
        Label6.Text = "Status : Porting"
        Dim startport As New Thread(AddressOf PortStart)
        startport.Start()
    End Sub
    Private Sub ShowBaseStock()
        Dim frm1Width As Integer = Me.Width

        baseport.Show()
        baseport.Location = New Point(Me.Location.X + frm1Width, Me.Location.Y)
    End Sub

    Private Sub PortStart()
        Dim ReadData As String = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\devices\lenovoa6000\main.json")
        Dim Item = Linq.JObject.Parse(ReadData)
        Dim progress As Integer = 0
        For Each Item2 As String In Item.Item("mirror").ToArray
            If Item2.Contains("=") Then
                Dim Command As String = GetStringBetween(Item2, "%", "%")
                If Command = "CLEAN" Then
                    Try
                        If My.Computer.FileSystem.DirectoryExists(port & "\" & Item2.Split("=")(0)) Then
                            progress += 1
                            SetLabelText("Status : Cleaning Directory : " & Item2.Split("=")(0) & " - " & Math.Round((progress / Item.Item("mirror").ToArray.Length) * 100, 2) & "%", Label6)
                            My.Computer.FileSystem.DeleteDirectory(port & "\" & Item2.Split("=")(0), FileIO.DeleteDirectoryOption.DeleteAllContents)
                            My.Computer.FileSystem.CreateDirectory(port & "\" & Item2.Split("=")(0))
                            SetLabelText("Status : Copying Directory : " & Item2.Split("=")(0) & " - " & Math.Round((progress / Item.Item("mirror").ToArray.Length) * 100, 2) & "%", Label6)
                            My.Computer.FileSystem.CopyDirectory(base & "\" & Item2.Split("=")(0), port & "\" & Item2.Split("=")(0))
                        Else
                            If My.Computer.FileSystem.FileExists(port & "\" & Item2.Split("=")(0)) Then
                                progress += 1
                                SetLabelText("Status : Deleting File : " & Item2.Split("=")(0) & " - " & Math.Round((progress / Item.Item("mirror").ToArray.Length) * 100, 2) & "%", Label6)
                                Kill(port & "\" & Item2.Split("=")(0))
                                SetLabelText("Status : Copying File : " & Item2.Split("=")(0) & " - " & Math.Round((progress / Item.Item("mirror").ToArray.Length) * 100, 2) & "%", Label6)
                                My.Computer.FileSystem.CopyFile(base & "\" & Item2.Split("=")(0), port & "\" & Item2.Split("=")(0), True)
                            End If
                        End If
                        GoTo nextend
                    Catch ex As Exception
                        ThrowException(ex, "Clean Copy Failed", ExceptionType.Critical)
                    End Try
                End If
            End If
            Try
                If My.Computer.FileSystem.DirectoryExists(port & "\" & Item2) Then
                    progress += 1
                    SetLabelText("Status : Copying Directory : " & Item2 & " - " & Math.Round((progress / Item.Item("mirror").ToArray.Length) * 100, 2) & "%", Label6)
                    My.Computer.FileSystem.CopyDirectory(base & "\" & Item2, port & "\" & Item2)
                Else
                    If My.Computer.FileSystem.FileExists(port & "\" & Item2) Then
                        progress += 1
                        SetLabelText("Status : Copying File : " & Item2 & " - " & Math.Round((progress / Item.Item("mirror").ToArray.Length) * 100, 2) & "%", Label6)
                        My.Computer.FileSystem.CopyFile(base & "\" & Item2, port & "\" & Item2.Split("=")(0), True)
                    End If
                End If
            Catch ex As Exception
                ThrowException(ex, "Copy Failed", ExceptionType.Critical)
            End Try
nextend:
        Next
        SetLabelText("Status : Porting Complete", Label6)
    End Sub
End Class
