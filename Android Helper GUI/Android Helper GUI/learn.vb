Imports System.Net

Public Class learn
    Dim server As String = "http://urgero.org/adbgui/learn"
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Dispose()
    End Sub


    Private Sub bw1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bw1.DoWork
        Try
            Button1.Enabled = False
            ListBox1.Items.Clear()
            ListBox1.Enabled = False

            Label4.Text = "Connecting...."
            Dim remoteUri As String = server + "/"
            Dim fileName As String = "software.lst"
            Dim myWebClient As New WebClient()
            Dim myStringWebResource = remoteUri + fileName
            myWebClient.DownloadFile(myStringWebResource, fileName)
            myWebClient.Dispose()

            Label4.Text = "Connected to: " + server
            ListBox1.Items.AddRange(System.IO.File.ReadAllLines("software.lst"))
            ListBox1.Enabled = True
            System.IO.File.Delete("software.lst")
            Button1.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub learn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        ' Dim server As String

        bw1.RunWorkerAsync()

    End Sub
    Public Function DownloadString(address As String) As String
        'Usage
        Dim instance As WebClient
        Dim address_file As String
        Dim returnValue As String

        returnValue = instance.DownloadString(address_file)
    End Function

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        'MsgBox(ListBox1.SelectedItem.ToString)
        Try
            Label4.Text = "Grabbing data from repository..."
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            Button1.Enabled = False
            'ListBox1.Enabled = False
            'ListBox1.ClearSelected()
            Dim selectedItem As String
            selectedItem = ListBox1.SelectedItem.ToString
            My.Computer.Network.DownloadFile(server + "/" + selectedItem + "/info.lst", "info.lst", String.Empty, String.Empty, False, 1000, True)
            My.Computer.Network.DownloadFile(server + "/" + selectedItem + "/download.lst", "download.lst", String.Empty, String.Empty, False, 1000, True)
            'Dim remoteUri As String = server + selectedItem + "/"
            'Dim fileName As String = "info.lst"
            'Dim myWebClient As New WebClient()
            'Dim myStringWebResource = remoteUri + fileName
            'myWebClient.DownloadFile(myStringWebResource, fileName)
            'Dim remoteUri2 As String = server + selectedItem + "/"
            'Dim fileName2 As String = "download.lst"
            'Dim myWebClient2 As New WebClient()
            'Dim myStringWebResource2 = remoteUri2 + fileName2
            'myWebClient2.DownloadFile(myStringWebResource2, fileName2)
            'myWebClient.Dispose()
            'myWebClient2.Dispose()

            Dim sr As New System.IO.StreamReader("info.lst")
            Dim item1 = sr.ReadToEnd
            sr.Close()
            Dim sr2 As New System.IO.StreamReader("download.lst")
            Dim item2 = sr2.ReadToEnd
            sr2.Close()
            TextBox1.Text = selectedItem
            TextBox2.Text = item1
            TextBox3.Text = item2
            If System.IO.File.Exists("info.lst") = True Then
                ListBox1.Enabled = True

            End If
            System.IO.File.Delete("info.lst")
            System.IO.File.Delete("download.lst")
            item1 = Nothing
            item2 = Nothing
            Label4.Text = "Connected to: " + server
            Button1.Enabled = True
            If TextBox3.Text = "N/A" Then
                Button1.Enabled = False
            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
EndLine2:

    End Sub

    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click
        bw1.RunWorkerAsync()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim usrDoc As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim Download = "/Download"

        Process.Start(TextBox3.Text)
    End Sub

    Private Sub OfficialADBHelpSiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OfficialADBHelpSiteToolStripMenuItem.Click
        Process.Start("http://developer.android.com/tools/help/adb.html")

    End Sub

    Private Sub ADBComandsExplainedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ADBComandsExplainedToolStripMenuItem.Click
        Process.Start("http://urgero.org/home/?page_id=5151")
    End Sub
End Class
