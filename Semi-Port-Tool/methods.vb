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
End Module
