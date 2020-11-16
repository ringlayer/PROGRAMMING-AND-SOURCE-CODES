﻿Imports MySql.Data.MySqlClient
Imports System.Math
Public Class form_master_data_customer
    Public data_obj As New data_object
    Public datatable_mysql = New DataTable
    Public basic_op As New basic_op

    Public Property saved_data As Integer
    Public Property page As Integer
    Public Property total_data As Integer
    Public Property max_page As Integer
    Public Sub prepare_data_pelengkap()
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            Me.total_data = data_obj.get_total_rows("master_customer")
            Me.max_page = Ceiling(Me.total_data / 10)
            Call select_combo_item()
            button_prev.Enabled = 0
            text_total_data.Text = Me.total_data
            text_total_page.Text = Me.max_page
            If max_page < 2 Then
                button_next.Enabled = 0
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub button_prev_Click(sender As Object, e As EventArgs) Handles button_prev.Click
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If Me.page > 1 Then
                Me.page = Me.page - 1
                Call LoadData(Me.page)
                button_next.Enabled = 1
                Call select_combo_item()

            Else
                button_prev.Enabled = 0
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub button_next_Click(sender As Object, e As EventArgs) Handles button_next.Click
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If Me.page < Me.max_page Then
                Me.page = Me.page + 1
                Call LoadData(Me.page)
                button_prev.Enabled = 1
                Call select_combo_item()
            Else
                button_next.Enabled = 0
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Public Sub select_combo_item()
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            combo_page.Items.Clear()
            For val As Integer = 1 To max_page
                combo_page.Items.Add(val)
                If Me.page = val Then
                    combo_page.SelectedItem = combo_page.Items(val - 1)
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub
    Public Sub LoadData(ByVal hal As Integer)
        Try
            Dim pg = (hal - 1) * 10
            Dim pg_str = pg.ToString()
            Dim sql As String
            sql = "SELECT id,nama,telpon,alamat,hp,email  FROM master_customer order by `id` desc limit " + pg_str + ",10"
            datatable_mysql = data_obj.load_data_for_datagrid(sql)
            DGView.DataSource = datatable_mysql
            DGView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            data_obj.con_mysql.Dispose()
            Dim i As Integer
            For i = 0 To DGView.Columns.Count - 1
                DGView.Columns.Item(i).SortMode = DataGridViewColumnSortMode.Programmatic
            Next i
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Private Sub form_master_data_customer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            basic_op.hide_id(label_id, text_id)
            form_master_data_customer_add.added_data = 0
            Me.saved_data = 0
            Me.page = 1
            Call prepare_data_pelengkap()
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        form_master_data_customer_add.Show()
        basic_op.setfokus(form_master_data_customer_add)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If form_master_data_customer_add.added_data = 1 Or Me.saved_data = 1 Then
                Call prepare_data_pelengkap()
                Call LoadData(page)
                Me.saved_data = 0
                form_master_data_customer_add.added_data = 0
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub
    Private Sub DGView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGView.CellClick
        selected_view(e)
    End Sub

    Private Sub DGView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGView.CellContentClick
        selected_view(e)
    End Sub


    Public Sub selected_view(e As DataGridViewCellEventArgs)
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If (e.RowIndex >= 0) And DGView.CurrentCell.Value <> Nothing Then
                Dim row As DataGridViewRow
                row = Me.DGView.Rows(e.RowIndex)
                text_id.Text = row.Cells("id").Value.ToString
                text_nama.Text = row.Cells("nama").Value.ToString
                text_telpon.Text = row.Cells("telpon").Value.ToString
                text_alamat.Text = row.Cells("alamat").Value.ToString
                text_hp.Text = row.Cells("hp").Value.ToString
                text_email.Text = row.Cells("email").Value.ToString
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If Len(text_nama.Text) < 1 Then
                MsgBox("nama harus diisi")
            ElseIf Len(text_alamat.Text) < 1 Then
                MsgBox("alamat harus diisi")
            ElseIf Len(text_telpon.Text) < 1 Then
                MsgBox("telpon harus diisi")
            Else
                Dim ret = 0
                Dim nama = text_nama.Text
                Dim alamat = text_alamat.Text
                Dim telpon = text_telpon.Text
                Dim hp = text_hp.Text
                Dim email = text_email.Text
                Dim id = text_id.Text
                Dim sql = "update `master_customer` set `nama` = '" + nama + "', `telpon` = '" + telpon + "', `alamat` = '" + alamat + "', `hp` = '" + hp + "', `email` = '" + email + "' where `id` like '" + id + "'"
                ret = data_obj.mysql_just_exec(sql)
                If ret = 1 Then
                    Me.saved_data = 1
                    Console.Beep(4000, 100)
                Else
                    MsgBox("data gagal disimpan")
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            Dim hapus = basic_op.konfirmasi_hapus("Apakah data customer akan dihapus ?", "Hapus Data Customer")
            If hapus = 1 Then
                Dim id = text_id.Text
                Dim res = 0
                res = data_obj.mysql_del("master_customer", id)
                If res = 0 Then
                    basic_op.gagal_delete()
                Else
                    Me.saved_data = 1
                    Console.Beep(4000, 100)
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If (Len(textcari_nama.Text) > 1) Or (Len(textcari_hp.Text) > 1) Then
                If (Len(textcari_nama.Text) > 1) Then
                    datatable_mysql = data_obj.load_data_pencarian(textcari_nama.Text, "nama", "master_customer")

                End If
                If (Len(textcari_hp.Text) > 1) Then
                    datatable_mysql = data_obj.load_data_pencarian(textcari_hp.Text, "hp", "master_customer")
                End If
                DGView.DataSource = datatable_mysql
                DGView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            End If
            Dim i As Integer
            For i = 0 To DGView.Columns.Count - 1
                DGView.Columns.Item(i).SortMode = DataGridViewColumnSortMode.Programmatic
            Next i
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub combo_page_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_page.SelectedIndexChanged
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            Me.page = combo_page.SelectedItem
            Call LoadData(Me.page)
            If Me.page = Me.max_page Then
                button_next.Enabled = 0
                button_prev.Enabled = 1
            End If
            If Me.page = 1 Then
                button_prev.Enabled = 0
                button_next.Enabled = 1
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub tombol_home_Click(sender As Object, e As EventArgs) Handles tombol_home.Click
        Me.page = 1
        Call LoadData(page)
    End Sub



    Private Overloads Sub textcari_nama_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles textcari_nama.KeyDown
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If Len(textcari_nama.Text) > 1 Then
                If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Enter Then
                    e.SuppressKeyPress = True
                    textcari_hp.Text = ""
                    datatable_mysql = data_obj.load_data_pencarian(textcari_nama.Text, "nama", "master_customer")
                    DGView.DataSource = datatable_mysql
                    DGView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue

                End If
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub


    Private Overloads Sub textcari_hp_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles textcari_hp.KeyDown
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            If Len(textcari_hp.Text) > 1 Then
                If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Enter Then
                    e.SuppressKeyPress = True
                    textcari_nama.Text = ""
                    datatable_mysql = data_obj.load_data_pencarian(textcari_hp.Text, "hp", "master_customer")
                    DGView.DataSource = datatable_mysql
                    DGView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub tombol_csv_Click(sender As Object, e As EventArgs) Handles tombol_csv.Click
        form_csv.additional_mark = ""
        form_csv.nama_tabel = "master_customer"
        form_csv.Show()
        basic_op.setfokus(form_csv)
    End Sub

    Private Sub tombol_delete_all_Click(sender As Object, e As EventArgs) Handles tombol_delete_all.Click
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Try
            Dim hapus = basic_op.konfirmasi_hapus("Apakah Anda yakin untuk menghapus seluruh data master customer ?", "Hapus Data Seluruh Master Customer")
            If hapus = 1 Then
                form_delete_all.table_name = "master_customer"
                form_delete_all.Show()
            End If
        Catch ex As Exception
            Console.WriteLine(methodName)
            Console.WriteLine(ex.Message)
            Console.WriteLine(ex.ToString)
        End Try
    End Sub
End Class