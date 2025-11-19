public class ReportingService
{
    private readonly string _connectionString;

    public ReportingService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Генерируем отчет по выбранному диапазону дат
    public async Task<List<FinancialReport>> GenerateReportAsync(DateTime startDate, DateTime endDate)
    {
        var reports = new List<FinancialReport>();
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand(
                @"SELECT * FROM financial_reports WHERE data_range_start BETWEEN @StartDate AND @EndDate",
                conn
            );
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    reports.Add(new FinancialReport
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        ReportType = reader.GetString(reader.GetOrdinal("report_type")),
                        ShiftId = reader.IsDBNull(reader.GetOrdinal("shift_id")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("shift_id")),
                        GeneratedBy = reader.IsDBNull(reader.GetOrdinal("generated_by")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("generated_by")),
                        DataRangeStart = reader.GetDateTime(reader.GetOrdinal("data_range_start")),
                        DataRangeEnd = reader.GetDateTime(reader.GetOrdinal("data_range_end")),
                        TotalRevenue = reader.GetDecimal(reader.GetOrdinal("total_revenue")),
                        CashRevenue = reader.GetDecimal(reader.GetOrdinal("cash_revenue")),
                        CardRevenue = reader.GetDecimal(reader.GetOrdinal("card_revenue")),
                        TotalOrders = reader.GetInt32(reader.GetOrdinal("total_orders"))
                    });
                }
            }
        }
        return reports;
    }

    // Экспорт отчета в Excel
    public async Task ExportToExcelAsync(List<FinancialReport> reports)
    {
        // Логика экспорта в Excel с использованием сторонних библиотек (например, EPPlus)
    }
}