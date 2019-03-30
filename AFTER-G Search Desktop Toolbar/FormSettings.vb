﻿Imports System.IO.File
Imports IWshRuntimeLibrary
Imports System.Net

Public Class FormSettings

    ''' <summary>
    ''' Ajoute le raccourci de démarrage automatique à l'ordinateur de l'utilisateur.
    ''' </summary>
    Sub AddStartup()
        'Dim Shell = New WshShell
        Dim StartupDir As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
        Dim objShell = New WshShell
        Dim objShortcut = objShell.CreateShortcut(StartupDir + "\SmartNet Search Bar.lnk")
        objShortcut.TargetPath = Application.ExecutablePath
        objShortcut.Save()
    End Sub

    ''' <summary>
    ''' Supprime le raccourci de démarrage automatique de l'ordinateur de l'utilisateur.
    ''' </summary>
    Sub RemoveStartup()
        Dim PathStartup As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
        System.IO.File.Delete(PathStartup & "\SmartNet Search Bar.lnk")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles AutoLaunchCheckBox.CheckedChanged
        If AutoLaunchCheckBox.Checked = True Then
            AddStartup()
        Else
            RemoveStartup()
        End If
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.Save()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case My.Settings.searchengine
            Case 1
                RadioButtonGoogle.Checked = True
                RadioButtonBing.Checked = False
                RadioButtonYahoo.Checked = False
                RadioButtonDuckDuckGo.Checked = False
                RadioButtonQwant.Checked = False
            Case 2
                RadioButtonGoogle.Checked = False
                RadioButtonBing.Checked = True
                RadioButtonYahoo.Checked = False
                RadioButtonDuckDuckGo.Checked = False
                RadioButtonQwant.Checked = False
            Case 3
                RadioButtonGoogle.Checked = False
                RadioButtonBing.Checked = False
                RadioButtonYahoo.Checked = True
                RadioButtonDuckDuckGo.Checked = False
                RadioButtonQwant.Checked = False
            Case 4
                RadioButtonGoogle.Checked = False
                RadioButtonBing.Checked = False
                RadioButtonYahoo.Checked = False
                RadioButtonDuckDuckGo.Checked = True
                RadioButtonQwant.Checked = False
            Case 5
                RadioButtonGoogle.Checked = False
                RadioButtonBing.Checked = False
                RadioButtonYahoo.Checked = False
                RadioButtonDuckDuckGo.Checked = False
                RadioButtonQwant.Checked = True
        End Select

        Try
            Dim PathStartup As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
            If Exists(PathStartup & "\SmartNet Search Bar.lnk") = True Then
                AutoLaunchCheckBox.Checked = True
            Else
                AutoLaunchCheckBox.Checked = False
            End If
        Catch ex As Exception
        End Try

        CurrentVersionLabel.Text = "Version actuelle : " + My.Application.Info.Version.ToString()
        Select Case UpdateAgent.IsUpdateAvailable()
            Case UpdateAgent.UpdateStatus.UpdateAvailable
                CheckUpdatesNowButton.Enabled = True
                CheckUpdatesNowButton.Text = "Nouvelle mise à jour disponible !"
            Case UpdateAgent.UpdateStatus.UpToDate
                CheckUpdatesNowButton.Enabled = False
                CheckUpdatesNowButton.Text = "SmartNet Square Note est à jour."
            Case UpdateAgent.UpdateStatus.SupportStatusOff
                My.Settings.autoupdate = False
                CheckUpdatesNowButton.Enabled = False
                CheckUpdatesNowButton.Text = "SmartNet Square Note est à jour."
            Case UpdateAgent.UpdateStatus.OSNotSupported
                My.Settings.autoupdate = False
                CheckUpdatesNowButton.Enabled = False
                CheckUpdatesNowButton.Text = "SmartNet Square Note est à jour."
        End Select
        My.Settings.Save()
        AutoUpdatesCheckBox.Checked = My.Settings.autoupdate

        BuiltInBrowserCheckBox.Checked = My.Settings.OpenBuiltInBrowser
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ImportFromPreviousVersionButton.Click
        My.Settings.Upgrade()
        My.Settings.Reload()
        ImportFromPreviousVersionButton.Text = "Merci de redémarrer le logiciel."
        ImportFromPreviousVersionButton.Enabled = False
    End Sub

    Private Sub RadioButtonGoogle_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonGoogle.CheckedChanged
        My.Settings.searchengine = 1
        FormSearchBar.PictureBoxSearchEngineLogo.Image = My.Resources.google
    End Sub

    Private Sub RadioButtonBing_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonBing.CheckedChanged
        My.Settings.searchengine = 2
        FormSearchBar.PictureBoxSearchEngineLogo.Image = My.Resources.bing
    End Sub

    Private Sub RadioButtonYahoo_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonYahoo.CheckedChanged
        My.Settings.searchengine = 3
        FormSearchBar.PictureBoxSearchEngineLogo.Image = My.Resources.yahoo
    End Sub

    Private Sub RadioButtonDuckDuckGo_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonDuckDuckGo.CheckedChanged
        My.Settings.searchengine = 4
        FormSearchBar.PictureBoxSearchEngineLogo.Image = My.Resources.duckduckgo
    End Sub

    Private Sub RadioButtonQwant_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonQwant.CheckedChanged
        My.Settings.searchengine = 5
        FormSearchBar.PictureBoxSearchEngineLogo.Image = My.Resources.qwant
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles CheckUpdatesNowButton.Click
        If UpdateAgent.IsUpdateAvailable = UpdateAgent.UpdateStatus.UpdateAvailable Then
            FormUpdater.ShowDialog()
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles AutoUpdatesCheckBox.CheckedChanged
        If AutoUpdatesCheckBox.Checked = True Then
            My.Settings.autoupdate = True
        End If
        If AutoUpdatesCheckBox.Checked = False Then
            My.Settings.autoupdate = False
            MsgBox("Avertissement : Les mises à jour permettent de corriger les beugs du logiciel et de lui apporter des nouvelles fonctionnalités. Si vous désactivez les mises à jour automatiques, ceci ne pourra pas être fait. Ceci n'est pas recommandé.", MsgBoxStyle.Exclamation, "Désactiver SmartNet Apps Updater")
        End If
    End Sub

    Private Sub BuiltInBrowserCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles BuiltInBrowserCheckBox.CheckedChanged
        If BuiltInBrowserCheckBox.Checked = True Then
            My.Settings.OpenBuiltInBrowser = True
        Else
            My.Settings.OpenBuiltInBrowser = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles ChangeBarColorButton.Click
        Dim ColorPickWindow As New ColorDialog
        If ColorPickWindow.ShowDialog() = vbOK Then
            My.Settings.color = ColorPickWindow.Color
            FormSearchBar.BackColor = My.Settings.color
            FormSearchBar.TextBox1.BackColor = My.Settings.color
        End If
    End Sub

    Private Sub DeleteHistoryButton_Click(sender As Object, e As EventArgs) Handles DeleteHistoryButton.Click
        Try
            BrowserForm.GeckoWebBrowser1.History.Clear()
        Catch ex As Exception
            MsgBox("Erreur : " + ex.Message, MsgBoxStyle.Critical, "SmartNet Search Bar")
        End Try
    End Sub

    Private Sub DeleteCookiesButton_Click(sender As Object, e As EventArgs) Handles DeleteCookiesButton.Click
        Try
            Gecko.CookieManager.RemoveAll()
        Catch ex As Exception
            MsgBox("Erreur : " + ex.Message, MsgBoxStyle.Critical, "SmartNet Search Bar")
        End Try
    End Sub

    Private Sub DeleteCache_Click(sender As Object, e As EventArgs) Handles DeleteCache.Click
        Try
            Gecko.Cache.CacheService.Clear(Gecko.Cache.CacheStoragePolicy.Anywhere)
        Catch ex As Exception
            MsgBox("Erreur : " + ex.Message, MsgBoxStyle.Critical, "SmartNet Search Bar")
        End Try
    End Sub

    Private Sub RestoreDefaultColorLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles RestoreDefaultColorLinkLabel.LinkClicked
        My.Settings.color = Color.LightSeaGreen
        FormSearchBar.BackColor = My.Settings.color
        FormSearchBar.TextBox1.BackColor = My.Settings.color
    End Sub
End Class