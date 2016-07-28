Public Class about
    Private Sub about_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text += My.Application.Info.Version.ToString
    End Sub
End Class