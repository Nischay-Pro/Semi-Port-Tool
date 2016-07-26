Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports Newtonsoft.Json
Public Class advanced
    Dim data As ListBox = Form1.deviceloc
    Dim fixdata As New ListBox
    Private Sub advanced_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each Directory As String In data.Items
            Try
                Dim ReadData As String = My.Computer.FileSystem.ReadAllText(Directory.Split("!")(0))
                Dim maindirectory As FileInfo = My.Computer.FileSystem.GetFileInfo(Directory.Split("!")(0))

                Dim Item = Linq.JObject.Parse(ReadData)
                For Each Item2 As String In Item.Item("advanced").ToArray
                    Dim ReadData2 As String = My.Computer.FileSystem.ReadAllText(maindirectory.DirectoryName & "\" & Item2)
                    Dim Item3 = Linq.JObject.Parse(ReadData2)
                    AddItem(Item3.Item("name").ToString, ComboBox1)
                    fixdata.Items.Add(Item3.Item("name").ToString & "!" & ReadData2)
                    ComboBox1.Enabled = True
                Next
            Catch ex As Exception
                ThrowException(ex, Nothing, ExceptionType.Warning)
                Exit Sub
            End Try
        Next
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        For Each Item As String In fixdata.Items
            If Item.Split("!")(0) = ComboBox1.Text Then
                Dim ReadData As String = Item.Split("!")(1)
                Dim Item2 = Linq.JObject.Parse(ReadData)
                Dim temp As String
                Dim curCulture As CultureInfo = Thread.CurrentThread.CurrentCulture
                Dim tInfo As TextInfo = curCulture.TextInfo()
                Label4.Text = "Name : " & Item2.Item("name").ToString
                Label5.Text = "Description : " & Item2.Item("description").ToString
                Label8.Text = "Developer : " & Item2.Item("developer").ToString
                temp = Item2.Item("type").ToString
                StrConv(temp, VbStrConv.ProperCase)
                Label6.Text = "Type : " & tInfo.ToTitleCase(temp)
                temp = Item2.Item("mirrortype").ToString
                StrConv(temp, VbStrConv.ProperCase)
                Label7.Text = "Copying Mode : " & tInfo.ToTitleCase(temp)
            End If
        Next
    End Sub
End Class