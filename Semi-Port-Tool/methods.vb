Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Module methods

    Public Enum ExceptionType
        Warning
        Critical
        Information
    End Enum
    Public Sub SetLabelText(ByVal text As String, ByVal Label As Control)
        If Label.InvokeRequired Then
            Label.Invoke(New SetText(AddressOf SetLabelText), text, Label)
        Else
            Label.Text = text
        End If
    End Sub
    Public Delegate Sub SetText(text As String, Label As Control)

    Public Sub AddItem(ByVal text As String, ByVal Control As ComboBox)
        If Control.InvokeRequired Then
            Control.Invoke(New AddItemText(AddressOf AddItem), text, Control)
        Else
            Control.Items.Add(text)
        End If
    End Sub
    Public Delegate Sub AddItemText(text As String, control As ComboBox)


    Public Sub ToggleEnableButton(ByVal opt As Boolean, ByVal control As Control)
        If control.InvokeRequired Then
            control.Invoke(New ToggleVisibility(AddressOf ToggleEnableButton), opt, control)
        Else
            control.Enabled = opt
        End If
    End Sub
    Public Delegate Sub ToggleVisibility(opt As Boolean, control As Control)

    Public Sub ToggleVisiblity(ByVal opt As Boolean, ByVal control As Control)
        If control.InvokeRequired Then
            control.Invoke(New ToggleVisibilityDele(AddressOf ToggleVisiblity), opt, control)
        Else
            control.Visible = opt
        End If
    End Sub
    Public Delegate Sub ToggleVisibilityDele(opt As Boolean, control As Control)
    Public Sub ThrowException(ByVal ex As Exception, ByVal Message As String, ByVal Type As ExceptionType)

    End Sub
    Public Function GetStringBetween(ByVal InputText As String,
                                 ByVal starttext As String,
                                 ByVal endtext As String)

        Dim startPos As Integer
        Dim endPos As Integer
        Dim lenStart As Integer
        startPos = InputText.IndexOf(starttext, StringComparison.CurrentCultureIgnoreCase)
        If startPos >= 0 Then
            lenStart = startPos + starttext.Length
            endPos = InputText.IndexOf(endtext, lenStart, StringComparison.CurrentCultureIgnoreCase)
            If endPos >= 0 Then
                Return InputText.Substring(lenStart, endPos - lenStart)
            End If
        End If
        Return "ERROR"
    End Function
    Public Function hash_generator(ByVal hash_type As String, ByVal file_name As String)

        Dim hash
        If hash_type.ToLower = "md5" Then
            hash = MD5.Create
        ElseIf hash_type.ToLower = "sha1" Then
            hash = SHA1.Create()
        ElseIf hash_type.ToLower = "sha256" Then
            hash = SHA256.Create()
        Else
            MsgBox("Unknown type of hash : " & hash_type, MsgBoxStyle.Critical)
            Return False
        End If

        Dim hashValue() As Byte

        Dim fileStream As FileStream = File.OpenRead(file_name)
        fileStream.Position = 0
        hashValue = hash.ComputeHash(fileStream)
        Dim hash_hex = PrintByteArray(hashValue)

        fileStream.Close()

        Return hash_hex

    End Function

    Public Function PrintByteArray(ByVal array() As Byte)

        Dim hex_value As String = ""

        Dim i As Integer
        For i = 0 To array.Length - 1

            hex_value += array(i).ToString("X2")

        Next i
        Return hex_value.ToLower

    End Function

    Public Function md5_hash(ByVal file_name As String)
        Return hash_generator("md5", file_name)
    End Function

    Public Function sha_1(ByVal file_name As String)
        Return hash_generator("sha1", file_name)
    End Function

    Public Function sha_256(ByVal file_name As String)
        Return hash_generator("sha256", file_name)
    End Function
End Module
