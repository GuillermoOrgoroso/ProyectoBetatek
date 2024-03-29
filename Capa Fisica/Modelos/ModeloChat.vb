﻿Public Class ModeloChat

    Inherits ModeloConexion

    Public Sub New(username As String, password As String)
        MyBase.New(username, password)

    End Sub

    Public Id As String
    Public Sesion As String
    Public De As String
    Public Para As String
    Public Mensaje As String
    Public FechaHora As String
    Public Leido As Boolean


    Public Sub Guardar()
        Me.Comando.CommandText =
            "
            INSERT INTO CHATEA(Sesion,De,Para,Texto,FechaHora)
            VALUES(" + Me.Sesion + ",'" + Me.De + "','" + Me.Para + "','" + Me.Mensaje + "',now())
            "

        Me.Comando.ExecuteNonQuery()
        Me.Conexion.Close()
    End Sub

    Public Function LeerMensajesNoLeidosParaPaciente()
        Me.Comando.CommandText =
            "
            SELECT 
	           m.de, m.fechahora, m.texto, m.idmensaje as id_mensaje, u.nombre as emisor, u.NombreUsuario
            FROM
	            Chatea m 
            JOIN
                Medico u 
                    ON m.de = u.NombreUsuario
            

            WHERE 
	            m.para = '" + Me.Para + "' AND
	            m.sesion = '" + Me.Sesion + "' AND 
	            m.leido = FALSE
            "
        Dim resultado As New DataTable
        resultado.Load(Me.Comando.ExecuteReader())

        Me.Conexion.Close()
        Return resultado

    End Function

    Public Function LeerMensajesNoLeidosParaMedico()
        Me.Comando.CommandText =
            "
            SELECT 
	           m.de, m.fechahora, m.texto, m.idmensaje as id_mensaje, u.nombre as emisor
            FROM
	            chatea m 
            JOIN
                Persona u 
                    ON m.de = u.Ci
            

            WHERE 
	            m.para = '" + Me.Para + "' AND
	            m.sesion = '" + Me.Sesion + "' AND 
	            m.leido = FALSE
            "
        Dim resultado As New DataTable
        resultado.Load(Me.Comando.ExecuteReader())

        Me.Conexion.Close()
        Return resultado

    End Function

    Public Sub MarcarLeido()
        Me.Comando.CommandText =
            "UPDATE Chatea SET Leido = true where IdMensaje = " + Me.Id + ";"
        Comando.ExecuteNonQuery()
        Me.Conexion.Close()

    End Sub

    Public Function ChatsNoRespondidos()
        Me.Comando.CommandText = "
            SELECT 
	           sesion,de As Cedula,FechaHora,Nombre
            FROM
	            chatea m 
            JOIN
                Persona u 
                    ON m.de = u.Ci
            

            WHERE 
                m.leido = FALSE AND
                m.para = 'Medico'     
            Group by Sesion
            "

        Reader = Comando.ExecuteReader()
        Return Reader

    End Function

    Public Function UltimaSesion()
        Me.Comando.CommandText = "SELECT MAX(sesion) FROM CHATEA"

        Return Comando.ExecuteScalar().ToString()
    End Function

End Class
