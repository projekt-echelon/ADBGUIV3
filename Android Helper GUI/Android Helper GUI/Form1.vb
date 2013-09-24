Imports RegawMOD.Android
Imports System.IO
Imports System.Management
Imports System.Runtime.InteropServices
Public Class Form1
    Dim android As AndroidController
    Dim device As Device
    Private WithEvents MyProcess As Process
    Private WithEvents MyProcess2 As Process
    Private Delegate Sub AppendOutputTextDelegate(ByVal text As String)
    Private Delegate Sub AppendOutputText2Delegate(ByVal text As String)
    Private WithEvents m_MediaConnectWatcher As ManagementEventWatcher
    Dim serial As String
    Dim verint As Integer = 52
    Dim VerString As String = "3.9"
    Private Sub ExiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExiToolStripMenuItem.Click
        End

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ofd1.ShowDialog()
            TextBox1.Text = ofd1.FileName

        Catch ex As Exception
            MsgBox("Invalid file name!", MsgBoxStyle.Exclamation, "ADB Helper")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim p As New ProcessStartInfo
        If ofd1.FileName = "" Then
            GoTo endInstall
        ElseIf ofd1.FileName = Nothing Then
            GoTo endInstall
        ElseIf File.Exists(ofd1.FileName) = False Then
            MsgBox("File cannot be accessed at this time! (Either it does not exsist or is open in another process.)", MsgBoxStyle.Exclamation, "Oops!")
            GoTo endInstall
        End If
        Try
            Button2.Enabled = False
            'PictureBox9.Visible = True
            'Label30.Visible = True
            device.InstallApk(ofd1.FileName)
            MsgBox("Finished with the installation!, you should now find the application on your device!", MsgBoxStyle.Exclamation, "Yay!")
        Catch ex As Exception
            MsgBox("There has been an error while trying to install this apk to the android device!", MsgBoxStyle.Exclamation, "Oops!")
        End Try
        Button2.Enabled = True
        PictureBox9.Visible = False
        Label30.Visible = False
        'p.FileName = "adb.exe"
        'Dim arg As String = ofd1.FileName
        'p.Arguments = "install " + arg
        'p.WindowStyle = ProcessWindowStyle.Normal
        'Process.Start(p)
endInstall:
    End Sub

    Private Sub InstallADBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InstallADBToolStripMenuItem.Click
        Try
            If MsgBox("This will download and install ADB and FASTBOOT into System32, continue?", MsgBoxStyle.YesNo, "Install core files?") = MsgBoxResult.Yes Then

                Try
                    My.Computer.Network.DownloadFile("http://urgero.org/adbgui/adb.exe", "adb.exe", vbNullString, vbNullString, True, 5000, True)
                    My.Computer.Network.DownloadFile("http://urgero.org/adbgui/AdbWinApi.dll", "AdbWinApi.dll", vbNullString, vbNullString, True, 5000, True)
                    My.Computer.Network.DownloadFile("http://urgero.org/adbgui/AdbWinUsbApi.dll", "AdbWinUsbApi.dll", vbNullString, vbNullString, True, 5000, True)
                    My.Computer.Network.DownloadFile("http://urgero.org/adbgui/fastboot.exe", "fastboot.exe", vbNullString, vbNullString, True, 5000, True)
                Catch ex As Exception
                    MsgBox("Could Not Update Completely!", MsgBoxStyle.Critical, "Error!")
                    MsgBox("Shutting down application!", MsgBoxStyle.Critical, "Error!")
                    End

                End Try

                Try
                    FileCopy("adb.exe", "C:\Windows\adb.exe")
                    System.IO.File.Delete("adb.exe")

                    FileCopy("AdbWinApi.dll", "C:\Windows\AdbWinApi.dll")
                    System.IO.File.Delete("AdbWinApi.dll")

                    FileCopy("AdbWinUsbApi.dll", "C:\Windows\AdbWinUsbApi.dll")
                    System.IO.File.Delete("AdbWinUsbApi.dll")

                    FileCopy("fastboot.exe", "C:\Windows\fastboot.exe")
                    System.IO.File.Delete("fastboot.exe")

                    MsgBox("Finished Downloading and Installing! You are now ready to use the program!", MsgBoxStyle.Information, "ADB GUI")

                Catch ex As Exception
                    MsgBox("Could not copy files to the system directory! Please make sure you run as admin!", MsgBoxStyle.Critical, "Error!")
                    MsgBox("Shutting down application!", MsgBoxStyle.Critical, "Error!")
                    End
                End Try

            Else
            End If
        Catch ex As Exception
            MsgBox("Please run this program as Administrator to gain access to System32!", MsgBoxStyle.Information, "ADB GUI")


        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "reboot", "bootloader")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Reboot " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "reboot", "recovery")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Reboot " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox(ex.Message)

            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "reboot", "fastboot")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Reboot " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox(ex.Message)

            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        'Command needs logcat

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If File.Exists("C:\Windows\adb.exe") Then
            android.Dispose()
        Else
            MsgBox("adb.exe must be installed to system first, please select that option from the menu under File.", MsgBoxStyle.Exclamation, "Oops!")
            GoTo EndLine
        End If
        Dim p As New ProcessStartInfo
        p.FileName = "adb.exe"
        Dim arg As String = ofd1.FileName
        p.Arguments = "shell"
        p.WindowStyle = ProcessWindowStyle.Normal
        Process.Start(p)
EndLine:

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If ofd2.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            GoTo endline
        End If
        Dim rec1 As String = ofd2.FileName
        If ofd2.FileName = "" Then
            GoTo endline
        End If
        If File.Exists("C:\Windows\fastboot.exe") Then
            Shell("fastboot flash recovery " + """" + ofd2.FileName + """", AppWinStyle.NormalFocus, True)
            MsgBox("Recovery Flash returned OK, try to reboot to recovery.", MsgBoxStyle.Information, "Done")
        End If
Endline:
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If ofd3.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            GoTo Endline
        End If
        Dim rec1 As String = ofd3.FileName
        If ofd3.FileName = "" Then
            GoTo endline
        End If
        If ofd3.FileName = "" Then
            GoTo endline
        End If
        If File.Exists("C:\Windows\fastboot.exe") Then
            Shell("fastboot flash boot " + """" + ofd3.FileName + """", AppWinStyle.NormalFocus, True)
            MsgBox("Boot Flash returned OK, try to reboot.", MsgBoxStyle.Information, "Done")
        End If
Endline:

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs)
        MsgBox("This program is brought to you by: Mitchell Urgero of URGERO.ORG (c)URGERO.ORG" + vbNewLine + " Application Version: " + VerString, MsgBoxStyle.Information, "About")

    End Sub

    

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Try
            android.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'If MsgBox("You must accept the following to run this program:" + vbNewLine + "1. You acknowledge that any warrenties implied or not are not effective." + vbNewLine + "2. This program uses commands that can infact Break/Brick devices if" + vbNewLine + "used improperly." + vbNewLine + "3. This program does not root devices, but rooted devices are compatible." + vbNewLine + "4. All trade marks are properties of their respective owners." + vbNewLine + "5. I am not responsible for any and all bricks." + vbNewLine + vbNewLine + "Do you agree?", MsgBoxStyle.YesNo, "Agreement") = MsgBoxResult.Yes Then
        'Else
        '    End
        'End If
        CheckForIllegalCrossThreadCalls = False
        Try
            If File.Exists("AndroidLib.dll") Then

            Else
                File.WriteAllBytes("AndroidLib.dll", My.Resources.AndroidLib)
            End If


        Catch ex As Exception
            MsgBox("Error setting up ADB Shell DLL: " + ex.Message + vbNewLine + "Please verify that AndroidLib.dll exsist, if not please download from urgero.org.", MsgBoxStyle.Critical, "Oops!")
        End Try
        Try
            If File.Exists("troubleshoot.rtf") Then
                RichTextBox2.LoadFile("troubleshoot.rtf")
            Else
                RichTextBox2.AppendText("The troubleshoot document has not been found.")
            End If
        Catch ex As Exception

        End Try

        Me.Text = "ADB Helper V: " + VerString
        Label36.Text = "This program is brought to you by: Mitchell Urgero of URGERO.ORG (c)URGERO.ORG" + vbNewLine + "Application Version: " + VerString + vbNewLine + "Build Number: " + verint.ToString
        'TabControl1.Enabled = False
        If Directory.Exists("backups") Then

        Else
            Directory.CreateDirectory("backups")
        End If
        If System.IO.File.Exists("C:\Windows\adb.exe") Then

        Else
            If MsgBox("It seems you are missing some important files from your System32 Directory that are required for this program to run. Would you like to download and install them now? them now?", MsgBoxStyle.YesNo, "Need further installation!") = MsgBoxResult.Yes Then

                Try
                    If MsgBox("This will download and install ADB and FASTBOOT into System32, continue?", MsgBoxStyle.YesNo, "Install core files?") = MsgBoxResult.Yes Then


                        Try
                            My.Computer.Network.DownloadFile("http://urgero.org/adbgui/adb.exe", "adb.exe", vbNullString, vbNullString, True, 5000, True)
                            My.Computer.Network.DownloadFile("http://urgero.org/adbgui/AdbWinApi.dll", "AdbWinApi.dll", vbNullString, vbNullString, True, 5000, True)
                            My.Computer.Network.DownloadFile("http://urgero.org/adbgui/AdbWinUsbApi.dll", "AdbWinUsbApi.dll", vbNullString, vbNullString, True, 5000, True)
                            My.Computer.Network.DownloadFile("http://urgero.org/adbgui/fastboot.exe", "fastboot.exe", vbNullString, vbNullString, True, 5000, True)
                            My.Computer.Network.DownloadFile("http://urgero.org/adbgui/AndroidLib.dll", "AndroidLib.dll", vbNullString, vbNullString, True, 5000, True)
                        Catch ex As Exception
                            MsgBox("Could Not Update Completely!", MsgBoxStyle.Critical, "Error!")
                            MsgBox("Shutting down application!", MsgBoxStyle.Critical, "Error!")
                            End

                        End Try

                        Try
                            FileCopy("adb.exe", "C:\Windows\adb.exe")
                            System.IO.File.Delete("adb.exe")

                            FileCopy("AdbWinApi.dll", "C:\Windows\AdbWinApi.dll")
                            System.IO.File.Delete("AdbWinApi.dll")

                            FileCopy("AdbWinUsbApi.dll", "C:\Windows\AdbWinUsbApi.dll")
                            System.IO.File.Delete("AdbWinUsbApi.dll")

                            FileCopy("fastboot.exe", "C:\Windows\fastboot.exe")
                            System.IO.File.Delete("fastboot.exe")

                            MsgBox("Finished Downloading and Installing! You are now ready to use the program!", MsgBoxStyle.Information, "ADB GUI")

                        Catch ex As Exception
                            MsgBox("Could not copy files to the system directory! Please make sure you run as admin!", MsgBoxStyle.Critical, "Error!")
                            MsgBox("Shutting down application!", MsgBoxStyle.Critical, "Error!")
                            End
                        End Try

                    Else
                    End If
                    Try
                        If File.Exists("AndroidLib.upd") Then
                        Else
                            If MsgBox("There is an update for a Library (DLL/Plugin) that needs to be updated in order for ADBGUI to work properly. Would you like to update? (Hitting no will close this application.)", MsgBoxStyle.YesNo, "DLL Update") = MsgBoxResult.Yes Then
                                My.Computer.Network.DownloadFile("http://urgero.org/adbgui/AndroidLib.dll", "AndroidLib.dll", vbNullString, vbNullString, True, 5000, True)
                                MsgBox("Update has completed!", MsgBoxStyle.Information, "Finished!")
                                File.Create("AndroidLib.upd")
                            Else
                                End
                            End If
                        End If
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                Catch ex As Exception
                    MsgBox("Please run this program as Administrator to gain access to System32!", MsgBoxStyle.Information, "ADB GUI")


                End Try
            Else
                GoTo resno
            End If
        End If
resno:
verline:


        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Public Sub StartDetection()
        ' __InstanceOperationEvent will trap both Creation and Deletion of class instances
        Dim query2 As String = "SELECT * FROM __InstanceOperationEvent WITHIN 10 WHERE TargetInstance ISA ""Win32_USBControllerDevice"""
        m_MediaConnectWatcher = New ManagementEventWatcher(query2)
        m_MediaConnectWatcher.Start()
    End Sub
    Private Sub Arrived(ByVal sender As Object, ByVal e As System.Management.EventArrivedEventArgs) Handles m_MediaConnectWatcher.EventArrived

        Dim mbo, obj As ManagementBaseObject

        'is  it a creation or deletion event
        mbo = CType(e.NewEvent, ManagementBaseObject)
        ' is it either created or deleted
        obj = CType(mbo("TargetInstance"), ManagementBaseObject)

        Select Case mbo.ClassPath.ClassName
            Case "__InstanceCreationEvent"
                Button14.PerformClick()


        End Select
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click

        If BackgroundWorker3.IsBusy = True Then
            'android.Dispose()
            'BackgroundWorker3.CancelAsync()
            'PictureBox1.Visible = False
            'Label14.Visible = False
            'BackgroundWorker3.RunWorkerAsync()
        Else
            BackgroundWorker3.RunWorkerAsync()
        End If


    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Process.Start("cmd.exe")
    End Sub


    Public Function GetProp( _
    key As String _
) As String

    End Function


    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Try
            If System.IO.File.Exists("odin422.exe") Then
                Process.Start("odin422.exe")
            Else
                If MsgBox("You must download Odin first, would you like to do that now?", MsgBoxStyle.YesNo, "Install Odin?") = MsgBoxResult.Yes Then
                    My.Computer.Network.DownloadFile("http://urgero.org/adbgui/odin422.exe", "odin422.exe", vbNullString, vbNullString, True, 5000, True)
                    Process.Start("odin422.exe")
                Else

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FDroidOpenSourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FDroidOpenSourceToolStripMenuItem.Click
        Try
            If MsgBox("This will download and install F-Droid Open Source Application to your to your phone, enabling a new open source repository on your device." + vbNewLine + "Continue?", MsgBoxStyle.YesNo, "Install F-Droid?") = MsgBoxResult.Yes Then
                My.Computer.Network.DownloadFile("http://f-droid.org/FDroid.apk", "FDroid.apk", vbNullString, vbNullString, True, 5000, True)
                Shell("adb install FDroid.apk")
            Else
                MsgBox("Installation canceled by user!", MsgBoxStyle.Information, "Info")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SourceCodeBrowserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SourceCodeBrowserToolStripMenuItem.Click
        Process.Start("https://github.com/mitchellurgero/ADBGUIV3")
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

continue1:
        Try
            Button14.Enabled = False
            Label14.Text = "Checking for a connected device..."
            android = AndroidController.Instance
            android.UpdateDeviceList()
            Label14.Text = "Start step 2..."
            If android.HasConnectedDevices Then
                serial = android.ConnectedDevices(0)
                device = android.GetConnectedDevice(serial)
                Label9.Text = "Battery Level: " + device.Battery.Level.ToString
                If device.HasRoot = True Then
                    Label10.Text = "SU Version: " + device.Su.Version.ToString
                Else
                    Label10.Text = "No su binary found!"
                End If
                Label11.Text = "Device State: " + device.State.ToString
                Label12.Text = "Device Serial: " + serial
                If device.BusyBox.Version = "" Then
                    Label15.Text = "BusyBox Version: No BusyBox Found!!"
                Else
                    Label15.Text = "BusyBox Version: " + device.BusyBox.Version
                End If
                If DeviceState.RECOVERY = True Then
                    MsgBox("Connected device has been detected in recovery mode! Some commands may not work!", MsgBoxStyle.Critical, "Recovery Detected!")

                End If

                Label7.Text = serial.ToUpper
                Label7.Text.ToUpper()

            Else
                ' MsgBox("No device found! Drivers may need to be installed first, or the device is not plugged in!", MsgBoxStyle.Critical, "Device not found!")
                Label7.Text = "No Devices Found!"
                Label29.Text = "No Devies Connected!"
            End If

            If Label7.Text = "No Devices Found!" Then
                Label9.Text = "No devices connected!"
                Label10.Text = ""
                Label11.Text = ""
                Label12.Text = ""
                Label15.Text = ""
            Else

            End If
            Try
                If android.HasConnectedDevices Then
                    serial = android.ConnectedDevices(0)
                    device = android.GetConnectedDevice(serial)

                    'Adds all of the build.prop keys to the listbox
                    ListBox2.Items.AddRange(device.BuildProp.Keys.ToArray)

                    'So no items are selected right away
                    ListBox2.SelectedIndex = -1
                Else
                    Label29.Text = "No Devies Connected, or an error occured while reading the build.prop file."
                End If
            Catch ex As Exception

            End Try


        Catch ex As Exception
            MsgBox("There has been an error communicating to the device. Here are a couple ways to troubleshoot the issue:" & vbNewLine & "1. Check the usb connection, unplug and plug back in the device." & vbNewLine & "2. Restarting the PC may solve the issue directly." & vbNewLine & "3. Make sure you have the correct drivers for your phone, and your version of windows. " & vbNewLine & "NOTE: Sometimes this error comes up because of a bug with android itself, if you see a serial number in the bottom left hand corner of the program, this error MAY be ignored, but take caution. Just because a serial number is there does not always mean that all the commands in this program will work. This error did come up for a reason." + vbNewLine + "Exact error: " + ex.Message, MsgBoxStyle.Exclamation, "Oops!")
        End Try
        Button14.Enabled = True
        'TabControl1.Enabled = True

        BackgroundWorker2.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            Label14.Text = "Checking for update from server..."
            Dim wc As New System.Net.WebClient()
            Dim address As String = "http://urgero.org/adbgui/adbversion.txt"
            Dim ver = wc.DownloadString(address)
            Dim servermessage As String = "http://urgero.org/adbgui/message.txt"
            Dim message = wc.DownloadString(servermessage)
            RichTextBox1.Text = message
            Dim UpdateMessage As String = "http://urgero.org/adbgui/updatemessage.txt"
            Dim uMessage = wc.DownloadString(UpdateMessage)
            TextBox8.Text = uMessage
            ' My.Computer.Network.DownloadFile("http://urgero.org/adbgui/message.rtf", "message.rtf", vbNullString, vbNullString, False, 5000, True)
            ' RichTextBox1.LoadFile("message.rtf")
            If ver > verint Then

                If MsgBox("An update has been found!" + vbNewLine + "Information about this update: " + vbNewLine + TextBox8.Text, MsgBoxStyle.YesNo, "Update Available!") = MsgBoxResult.Yes Then
                    Try

                        Process.Start("update.exe")
                        End
                    Catch ex1 As Exception
                        loading.Visible = False
                        MsgBox("Cannot start updater application!")

                    End Try
                End If
            End If

            Label14.Text = "Finished Loading Resources!"
        Catch ex As Exception
            loading.Visible = False
            'MsgBox("Cannot reach the update server, please try updating later!", MsgBoxStyle.Information, "Oops!")
        End Try
        PictureBox1.Visible = False
        Label14.Visible = False

    End Sub

    Private Sub BackgroundWorker3_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork

continue1:
        Try
            Button14.Enabled = False
            PictureBox1.Visible = True
            Label14.Text = "Checking for a connected device..."
            Label14.Visible = True
            Dim serial As String
            android = AndroidController.Instance
            android.UpdateDeviceList()
            If android.HasConnectedDevices Then
                serial = android.ConnectedDevices(0)
                device = android.GetConnectedDevice(serial)
                Label7.Text = serial.ToUpper
                Label7.Text.ToUpper()
                Label9.Text = "Battery Level: " + device.Battery.Level.ToString
                If device.HasRoot = True Then
                    Label10.Text = "SU Version: " + device.Su.Version.ToString
                Else
                    Label10.Text = "No su binary found!"
                End If
                Label11.Text = "Device State: " + device.State.ToString
                Label12.Text = "Device Serial: " + serial
                If device.BusyBox.Version = "" Then
                    Label15.Text = "BusyBox Version: No BusyBox Found!!"
                Else
                    Label15.Text = "BusyBox Version: " + device.BusyBox.Version
                End If
            Else
                'MsgBox("No device found! Drivers may need to be installed first, or the device is not plugged in!", MsgBoxStyle.Critical, "Device not found!")
                Label7.Text = "No Devices Found!"
                Label29.Text = "No Devies Connected!"
            End If
            If DeviceState.RECOVERY = True Then
                MsgBox("Connected device has been detected in recovery mode! Some commands may not work!", MsgBoxStyle.Critical, "Recovery Detected!")

            End If
        Catch ex As Exception
            MsgBox("Error finding a device: " & ex.Message)
        End Try
      
            Try
                If android.HasConnectedDevices Then
                    serial = android.ConnectedDevices(0)
                    device = android.GetConnectedDevice(serial)

                    'Adds all of the build.prop keys to the listbox
                    ListBox2.Items.AddRange(device.BuildProp.Keys.ToArray)

                    'So no items are selected right away
                    ListBox2.SelectedIndex = -1
                Else
                    Label29.Text = "No Devies Connected, or an error occured while reading the build.prop file."
                End If
            Catch ex As Exception

            End Try



        If Label7.Text = "No Devices Found!" Then
            Label9.Text = "No devices connected!"
            Label10.Text = ""
            Label11.Text = ""
            Label12.Text = ""
            Label15.Text = ""
        Else

        End If
        Label14.Text = "Finished Loading Resources!"
        PictureBox1.Visible = False
        Label14.Visible = False
        Button14.Enabled = True

    End Sub

    Private Sub MyAndroidApplicationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MyAndroidApplicationsToolStripMenuItem.Click
        Process.Start("https://play.google.com/store/search?q=urgero")
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbShellCommand(device, True, "am", "kill-all")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Killed background processes: " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox("Error Uninstalling Application: " + ex.Message, MsgBoxStyle.Exclamation, "Oops!")

            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        Dim arg As String = ofd1.FileName
        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "push", """" + arg + """" + " " + """" + TextBox4.Text + """")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Push: " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If

    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Try
            ofd1.ShowDialog()
            TextBox3.Text = ofd1.SafeFileName

        Catch ex As Exception
            MsgBox("Invalid file name!", MsgBoxStyle.Exclamation, "ADB Helper")
        End Try
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        If TextBox5.TextLength = TextBox5.MaxLength Then
            TextBox5.Text = ""
        End If
        MyProcess.StandardInput.WriteLine(TextBox6.Text)
        MyProcess.StandardInput.Flush()
        TextBox6.Text = ""
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        Try
            If Label7.Text = "No Device Found!" Then
                GoTo badLine2
            Else

            End If
                If Label7.Text = "No Devices Found!" Then
                    GoTo badLine2
                Else

                End If
                If File.Exists("C:\Windows\adb.exe") = True Then
                    GoTo startLine
                Else
                    GoTo badLine
                End If
startLine:
                AcceptButton = Button23

                MyProcess = New Process

                With MyProcess.StartInfo
                .FileName = "CMD.EXE"
                '.Arguments = "SHELL"
                .UseShellExecute = False
                    .CreateNoWindow = True
                    .RedirectStandardInput = True
                    .RedirectStandardOutput = True
                    .RedirectStandardError = True
                End With

                MyProcess.Start()

                MyProcess.BeginErrorReadLine()      'start async read on stderr
                MyProcess.BeginOutputReadLine()     'start async read on stdout
            ' MyProcess.StandardInput.WriteLine("cls") 'send an EXIT command to the Command Prompt
                MyProcess.StandardInput.Flush()
                AppendOutputText("Process Started at: " & MyProcess.StartTime.ToString)
                TextBox6.Enabled = True
                Button23.Enabled = True
                Button25.Enabled = True
            Button24.Enabled = False
            TextBox6.Focus()
            TextBox6.Text = "adb shell"
            Button23.PerformClick()

                GoTo finStart
badLine:
            AppendOutputText("Please make sure ADB is installed to the system dir!" + vbNewLine)
                GoTo finStart

badLine2:
            AppendOutputText("An android device needs to be connected first!" + vbNewLine)
finStart:
            Catch ex As Exception
                MsgBox("Error starting ADB SHELL!!", MsgBoxStyle.Critical, "Oops!")
            End Try

    End Sub


    Private Sub AppendOutputText(ByVal text As String)

        If TextBox5.InvokeRequired Then
            Dim myDelegate As New AppendOutputTextDelegate(AddressOf AppendOutputText)
            Me.Invoke(myDelegate, text)
        Else
            TextBox5.AppendText(text)
        End If

    End Sub
    Private Sub MyProcess_OutputDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles MyProcess.OutputDataReceived

        AppendOutputText(vbCrLf & e.Data)

    End Sub
    Private Sub MyProcess_ErrorDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles MyProcess.ErrorDataReceived

        AppendOutputText(vbCrLf & e.Data)

    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        Try
            MyProcess.StandardInput.WriteLine("EXIT") 'send an EXIT command to the Command Prompt
            MyProcess.StandardInput.Flush()

            TextBox6.Enabled = False
            Button23.Enabled = False
            Button25.Enabled = False
            Button24.Enabled = True
            Button26.Enabled = True
            TextBox5.Text = "Service Stopped."
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        Try
            'If Label7.Text = "No Device Found!" Then
            '    GoTo badLine2
            'Else

            'End If
            'If Label7.Text = "No Devices Found!" Then
            '    GoTo badLine2
            'Else

            'End If
            'If File.Exists("C:\Windows\adb.exe") = True Then
            '    GoTo startLine
            'Else
            '    GoTo badLine
            'End If
startLine:
            AcceptButton = Button23

            MyProcess = New Process

            With MyProcess.StartInfo
                .FileName = "CMD.EXE"
                '.Arguments = "SHELL"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                .RedirectStandardError = True
            End With

            MyProcess.Start()

            MyProcess.BeginErrorReadLine()      'start async read on stderr
            MyProcess.BeginOutputReadLine()     'start async read on stdout
            ' MyProcess.StandardInput.WriteLine("cls") 'send an EXIT command to the Command Prompt
            MyProcess.StandardInput.Flush()
            AppendOutputText("Process Started at: " & MyProcess.StartTime.ToString)
            TextBox6.Enabled = True
            Button23.Enabled = True
            Button25.Enabled = True
            Button26.Enabled = False
            Button24.Enabled = False
            TextBox6.Focus()
            GoTo finStart
badLine:
            AppendOutputText("Please make sure ADB is installed to the system dir!" + vbNewLine)
            GoTo finStart

badLine2:
            AppendOutputText("An android device needs to be connected first!" + vbNewLine)
finStart:
        Catch ex As Exception
            MsgBox("Error starting ADB SHELL!!", MsgBoxStyle.Critical, "Oops!")
        End Try
    End Sub

    Private Sub ResetLoginScreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetLoginScreenToolStripMenuItem.Click
        Try
            My.Computer.Registry.CurrentUser.CreateSubKey("urgeroADBGUI")
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\urgeroADBGUI",
              "current_username", "")
            My.Computer.Registry.CurrentUser.CreateSubKey("urgeroADBGUI")
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\urgeroADBGUI",
              "current_p", "")
            MsgBox("Login screen and information has been reset!", MsgBoxStyle.Information, "Done!")
        Catch ex As Exception
            MsgBox("Error reading from registry, please run this program as administrator!", MsgBoxStyle.Critical, "Oops!")
        End Try
    End Sub


    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        sfd1.ShowDialog()
        If sfd1.FileName = "" Then

        Else
            TextBox5.AppendText("File Saved at: " & DateTime.Now)
            My.Computer.FileSystem.WriteAllText(sfd1.FileName, TextBox5.Text, False)

        End If
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        Dim objRandom As New System.Random
        Dim fileDateTime As String = DateTime.Now.ToString("yyyyMMdd") & "_" & DateTime.Now.ToString("HHmmss")
        Dim backupInteger As Integer = Math.Round(objRandom.NextDouble() * 163, 4)

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "backup", "-apk -all -f " + """" + Application.StartupPath & "\backups\backup_" & fileDateTime.ToString & ".ab" + """")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("The device will tell you when backup has completed. ", MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        restore1.InitialDirectory = Application.StartupPath & "\backups\"
        restore1.ShowDialog()
        If restore1.FileName = "" Then
        Else
            If MsgBox("Are you sure you would like to restore: " & restore1.FileName & " ?", MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
                If android.HasConnectedDevices Then
                    Try
                        Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "restore", """" + restore1.FileName + """")
                        Dim test = Adb.ExecuteAdbCommand(adbcmd)
                        MsgBox("The Device Will Let you know when the restore is complete.")
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Else
                    MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
                End If
            End If
        End If

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub


    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub

    Private Sub PictureBox6_Click(sender As Object, e As EventArgs)
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub

    Private Sub PictureBox7_Click(sender As Object, e As EventArgs)
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub

    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RKWA3MZANRADC")
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Label29.Text = device.BuildProp.GetProp(ListBox2.SelectedItem.ToString())
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        Dim p As New ProcessStartInfo
        If ofd1.FileName = "" Then
            GoTo endInstall
        ElseIf ofd1.FileName = Nothing Then
            GoTo endInstall
        ElseIf File.Exists(ofd1.FileName) = False Then
            MsgBox("File cannot be accessed at this time! (Either it does not exsist or is open in another process.)", MsgBoxStyle.Exclamation, "Oops!")
            GoTo endInstall
        End If
        MsgBox("With this method we have opened up a new window in the background to install the apk file. If it is still open you will find it in the task bar." & vbNewLine & "This method should work if the above will fail. the only down fall is there is no confirmation of installation or failure.", MsgBoxStyle.Information, "OK")
        p.FileName = "adb.exe"
        Dim arg As String = ofd1.FileName
        p.Arguments = "install " + """" + arg + """"
        p.WindowStyle = ProcessWindowStyle.Normal
        Process.Start(p)
endInstall:
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs)
        Try

            MyProcess2.Kill()
            MyProcess2.Dispose()
            Dim proc = Process.GetProcessesByName("adb")
            For i As Integer = 0 To proc.Count - 1
                proc(i).Kill()
            Next i
        Catch ex As Exception

        End Try



    End Sub

    Private Sub LearningCenterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LearningCenterToolStripMenuItem.Click
        learn.Show()
    End Sub

    Private Sub GetHelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetHelpToolStripMenuItem.Click
        Form3.Show()
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub
    Private Sub RichTextBox2_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        'Command Needs logcat
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/ameer1234567890/OnlineNandroid/wiki/Supported-Devices")
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        android.Dispose()
        Label40.Text = "Please wait while launching Application Manager..."

        uninstallapp.Show()
        Label40.Text = "Please note: After you are done managing packages, please remember to close the window" + vbNewLine + "and hit scan when you are done!" + vbNewLine + "Also, Application Manager is in pre-beta release, some functions may not work, or may not work on some" + vbNewLine + "devices. Please use caution when using the application manager."
    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "reboot", "fastboot")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Reboot " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception

            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "reboot", "fastboot")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Reboot " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception

            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If android.HasConnectedDevices Then
            Try
                Dim adbcmd As AdbCommand = Adb.FormAdbCommand(device, "reboot", "fastboot")
                Dim test = Adb.ExecuteAdbCommand(adbcmd)
                MsgBox("Reboot " + test.ToString, MsgBoxStyle.Information, "Done")
            Catch ex As Exception
                MsgBox(ex.Message)

            End Try
        Else
            MsgBox("An Android device must be connected to use this command!", MsgBoxStyle.Information, "Oops!")
        End If

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            If File.Exists("C:\Windows\fastboot.exe") Then
                Shell("fastboot oem get_identifier_token >> token.txt", AppWinStyle.Hide, True)
                If File.Exists("token.txt") Then
                    Process.Start("token.txt")
                Else
                    MsgBox("Error writing token.txt to disk!", MsgBoxStyle.Exclamation, "Oops!")
                End If
            Else
                MsgBox("adb needs to be installed to system before we can continue. Please select File -> Install ADB", MsgBoxStyle.Exclamation, "Oops!")

            End If
        Catch ex As Exception
            MsgBox("Error: " + ex.Message)

        End Try
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click
        Process.Start("http://htcdev.com/bootloader")
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
End Class
