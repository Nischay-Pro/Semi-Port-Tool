Imports Newtonsoft.Json
Imports System.Threading
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text += My.Application.Info.Version.ToString
        wait(2000)
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
    Private Sub SetLabelText(ByVal text As String, ByVal Label As Control)
        If Label.InvokeRequired Then
            Label.Invoke(New SetText(AddressOf SetLabelText), text, Label)
        Else
            Label.Text = text
        End If
    End Sub
    Public Delegate Sub SetText(text As String, Label As Control)

    Private Sub AddItem(ByVal text As String)
        If ComboBox1.InvokeRequired Then
            ComboBox1.Invoke(New AddItemText(AddressOf AddItem), text)
        Else
            ComboBox1.Items.Add(text)
        End If
    End Sub
    Public Delegate Sub AddItemText(text As String)
    Dim devicelist As Integer
    Private Sub StartupEngine()
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
                AddItem(Item.Item("goodname").ToString)
                devicelist += 1
            Catch ex As Exception
                ThrowException(ex, Nothing, ExceptionType.Warning)
            Exit Sub
            End Try

        Next
    End Sub

    Private Sub ThrowException(ByVal ex As Exception, ByVal Message As String, ByVal Type As ExceptionType)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        advanced.Show()
    End Sub
End Class
