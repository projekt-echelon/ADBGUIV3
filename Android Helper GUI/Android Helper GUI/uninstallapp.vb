Imports RegawMOD.Android

Public Class uninstallapp
    Dim android As AndroidController
    Dim device As Device
    Dim serial As String

    Private Sub uninstallapp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.Label7.Text = "No Devices Found!" Then
            MsgBox("An Android device needs to be connected to be able to manage it's applications! Closing App Manager!", MsgBoxStyle.Information, "Oops!")
            Me.Dispose()
        Else
            LoadApps()
        End If

    End Sub
    Private Sub LoadApps()
        ListBox1.Items.Clear()

        Try
            android = AndroidController.Instance
            android.UpdateDeviceList()
            If android.HasConnectedDevices Then
                serial = android.ConnectedDevices(0)
                device = android.GetConnectedDevice(serial)
                If device.HasRoot = True Then
                    Dim adbcmd As AdbCommand = Adb.FormAdbShellCommand(device, True, "pm", "list packages -3")
                    Dim sw As New System.IO.StreamWriter("packages.txt")
                    sw.WriteLine(Adb.ExecuteAdbCommand(adbcmd))
                    sw.Close()
                    ListBox1.Items.AddRange(Split(My.Computer.FileSystem.ReadAllText("packages.txt"), vbNewLine))
                Else
                    MsgBox("Your device needs root access to use the Windows Android Package manager Addon!", MsgBoxStyle.Information, "Oops!")
                    Me.Dispose()

                End If
            End If


            'Shell("adb shell su pm list packages -3 >> " + """" + Application.StartupPath + "\packages.txt""", AppWinStyle.Hide, True)


            'My.Computer.FileSystem.DeleteFile("packages.txt")
            Form1.Label7.Text = "Please rescan to reconnect.."
        Catch ex As Exception
            MsgBox("Error reading package list file! " + ex.Message)
        End Try
    End Sub
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Dispose()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim replacedtext As String = ListBox1.SelectedItem.ToString.Replace("package:", "")
        Label2.Text = replacedtext
        WebBrowser1.Navigate("https://play.google.com/store/apps/details?id=" + replacedtext)

    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Try
            Dim titleText = WebBrowser1.DocumentTitle.Replace("- Android Apps on Google Play", "")
            If titleText = "Not Found" Then
                MsgBox("Oops! Looks like this application is not on the android market! A lot of information will be missing here.", MsgBoxStyle.Exclamation, "Oops!")
                Label1.Text = Label2.Text
            ElseIf titleText = "Navigation Canceled" Then
                MsgBox("Could not find an internet connection! Internet Connection is needed to run Application manager!", MsgBoxStyle.Exclamation, "Oops!")
                Me.Dispose()

            Else
                Dim htmlDocument As HtmlDocument = Me.WebBrowser1.Document
                Dim htmlElementCollection As HtmlElementCollection = htmlDocument.Images
                Dim ImagesFound As Integer = 0
                'For Each htmlElement As HtmlElement In htmlElementCollection
                '    Dim imgUrl As String = htmlElement.GetAttribute("src")
                '    Label1.Text = titleText
                '    PictureBox1.ImageLocation = imgUrl
                'Next
                Dim appicon As String = WebBrowser1.Document.Images(0).GetAttribute("src")
                PictureBox1.ImageLocation = appicon
                'MsgBox(appicon)
                Label1.Text = titleText
            End If



        Catch ex As Exception

        End Try


    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim replacedtext As String = ListBox1.SelectedItem.ToString.Replace("package:", "")
        Process.Start("https://play.google.com/store/apps/details?id=" + replacedtext)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim replacedtext As String = ListBox1.SelectedItem.ToString.Replace("package:", "")
            Dim arg As String = "uninstall " + replacedtext
            Dim adbcmd As AdbCommand = Adb.FormAdbShellCommand(device, True, "pm", arg)
            Dim test = Adb.ExecuteAdbCommand(adbcmd)
            MsgBox("Uninstall " + test.ToString, MsgBoxStyle.Information, "Done")
            LoadApps()

            'MsgBox("Application: " + Label1.Text + " has been uninstalled!", MsgBoxStyle.Information, "Uninstalled!")
        Catch ex As Exception
            MsgBox("Error Uninstalling Application: " + ex.Message, MsgBoxStyle.Exclamation, "Oops!")

        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ofd1.ShowDialog()
        If ofd1.FileName = "" Then
        Else
            TextBox1.Text = ofd1.FileName
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try

            Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "install", """" + TextBox1.Text + """")
            Dim test = device.InstallApk(TextBox1.Text)
            If test = True Then
                MsgBox("Install successful!", MsgBoxStyle.Information, "Yay!")
            Else
                MsgBox("Install Failure!", MsgBoxStyle.Information, "Oops!")
            End If
            'MsgBox("Application: " + ofd1.SafeFileName + " has been installed!", MsgBoxStyle.Information, "Installed!")
            LoadApps()
        Catch ex As Exception
            MsgBox("Error Installing Application: " + ex.Message, MsgBoxStyle.Exclamation, "Oops!")

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim replacedtext As String = ListBox1.SelectedItem.ToString.Replace("package:", "")
        Dim arg As String = "clear " + replacedtext
        Dim adbcmd As AdbCommand = Adb.FormAdbShellCommand(device, True, "pm", arg)
        Dim test = Adb.ExecuteAdbCommand(adbcmd)
        MsgBox("Clear Data " + test.ToString, MsgBoxStyle.Information, "Done")
    End Sub
End Class