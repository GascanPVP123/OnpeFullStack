using Microsoft.Data.SqlClient;
using System.Data;

namespace OnpeFullStack.DAO
{
    public class OnpeDAO
    {

        private readonly string cadena;

        public OnpeDAO(IConfiguration configuration)
        {
            cadena = configuration.GetConnectionString("DefaultConnection") ?? "";
        }


        public DataTable GetResumenGeneral()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = @"
                SELECT 
                    SUM(TotalVotantes) AS Asistentes,
                    CAST(ROUND((SUM(CAST(TotalVotantes AS FLOAT)) * 100.0 / NULLIF(SUM(ElectoresHabiles), 0)), 3) AS DECIMAL(10,3)) AS PorcentajeParticipacion,
                    SUM(ElectoresHabiles - TotalVotantes) AS Ausentes,
                    CAST(ROUND(((SUM(CAST(ElectoresHabiles AS FLOAT)) - SUM(CAST(TotalVotantes AS FLOAT))) * 100.0 / NULLIF(SUM(ElectoresHabiles), 0)), 3) AS DECIMAL(10,3)) AS PorcentajeAusentismo,
                    SUM(ElectoresHabiles) AS ElectoresHabiles
                FROM GrupoVotacion";

                    SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("********* ERROR REAL DE SQL *********");
                Console.WriteLine(ex.Message);
                Console.WriteLine("*************************************");
            }
            return dt;
        }

        public DataTable GetResultadosPresidenciales()
        {
            DataTable dt = new DataTable();
            string cadena = "Server=sql8006.site4now.net;Database=db_ac6841_onpe;User Id=db_ac6841_onpe_admin;Password=Senati2026@;TrustServerCertificate=True;";

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = @"
                    SELECT 
                        ISNULL(SUM(P1), 0) AS VotosP1, 
                        ISNULL(SUM(P2), 0) AS VotosP2,
                        ISNULL(SUM(VotosBlancos), 0) AS VotosBlanco, 
                        ISNULL(SUM(VotosNulos), 0) AS VotosNulo,
                        ISNULL(SUM(P1 + P2), 0) AS TotalValidos,
                        ISNULL(SUM(P1 + P2 + VotosBlancos + VotosNulos), 0) AS TotalEmitidos,
        
                        CAST(ISNULL(SUM(P1), 0) * 100.0 / NULLIF(SUM(P1 + P2), 0) AS DECIMAL(10,3)) AS PorcentajeP1,
                        CAST(ISNULL(SUM(P2), 0) * 100.0 / NULLIF(SUM(P1 + P2), 0) AS DECIMAL(10,3)) AS PorcentajeP2,
        
                        CAST(ISNULL(SUM(TotalVotantes), 0) * 100.0 / NULLIF(SUM(ElectoresHabiles), 0) AS DECIMAL(10,3)) AS PorcParticipacion,
                        CAST(100 - (ISNULL(SUM(TotalVotantes), 0) * 100.0 / NULLIF(SUM(ElectoresHabiles), 0)) AS DECIMAL(10,3)) AS PorcAusentismo,

                        COUNT(*) AS TotalActas,
                        SUM(CASE WHEN idEstadoActa IN (1,2) THEN 1 ELSE 0 END) AS Procesadas,
                        SUM(CASE WHEN idEstadoActa = 1 THEN 1 ELSE 0 END) AS Contabilizadas,
                        ISNULL(SUM(TotalVotantes), 0) AS CiudadanosVotaron,
                        ISNULL(SUM(ElectoresHabiles), 0) AS ElectoresHabiles
                    FROM GrupoVotacion";

                    SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("--- ERROR DE CONEXIÓN ONPE ---");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return dt;
        }

        public DataTable GetResumenGeneralPresidencial()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    string sql = @"
                SELECT 
                    SUM(P1) AS VotosPPK, 
                    CAST(ROUND((SUM(CAST(P1 AS FLOAT)) * 100.0 / NULLIF(SUM(P1 + P2), 0)), 3) AS DECIMAL(10,3)) AS PorcPPK,
                    SUM(P2) AS VotosKeiko, 
                    CAST(ROUND((SUM(CAST(P2 AS FLOAT)) * 100.0 / NULLIF(SUM(P1 + P2), 0)), 3) AS DECIMAL(10,3)) AS PorcKeiko
                FROM GrupoVotacion";

                    SqlDataAdapter da = new SqlDataAdapter(sql, cn);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Resumen Presidencial: " + ex.Message);
            }
            return dt;
        }

    }

}