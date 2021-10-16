Public Class Controls 

    Private Sub Controls_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        headLine.X2 = Me.Width - 20
    End Sub

    Private Sub Controls_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class