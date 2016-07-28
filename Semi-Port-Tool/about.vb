Public Class about
    Private Sub about_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text += My.Application.Info.Version.ToString
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Process.Start("https://github.com/Nischay-Pro/Semi-Port-Tool")
    End Sub
End Class