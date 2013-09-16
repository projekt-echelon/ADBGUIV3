Public Class Form3

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Dispose()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Me.Text = WebBrowser1.DocumentTitle
    End Sub

    Private Sub BackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackToolStripMenuItem.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub ForwardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForwardToolStripMenuItem.Click
        WebBrowser1.GoForward()
    End Sub


End Class