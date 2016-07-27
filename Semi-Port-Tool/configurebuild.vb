Public Class configurebuild
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub configurebuild_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox2.Checked = Form1.IgnoreFileFolder
        CheckBox1.Checked = Form1.DiffTool
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Form1.IgnoreFileFolder = CheckBox2.Checked
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Form1.DiffTool = CheckBox1.Checked
    End Sub
End Class