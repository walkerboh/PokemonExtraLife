Highcharts.chart("HourlyChart",
    {
        chart: {
            type: "column"
        },
        title: {
            text: "Donations By Hour"
        },
        legend: {
            enabled: false
        },
        data: {
            csvURL: "https://localhost:44369/analytics/hourlychart",
            enablePolling: true,
            dataRefreshRate: 5
        },
        xAxis: {
            type: "category"
        },
        yAxis: [
            {
                title: false
            }, {
                title: false,
                opposite: true
            }
        ],
        series: [
            {
                yAxis: 0
            },
            {
                yAxis: 1
            }
        ]
    });

Highcharts.chart("GymChart",
    {
        chart: {
            type: "column"
        },
        title: {
            text: "Donations By Gym"
        },
        legend: {
            enabled: false
        },
        data: {
            csvURL: "https://localhost:44369/analytics/gymchart",
            enablePolling: true,
            dataRefreshRate: 5
        },
        xAxis: {
            type: "category"
        },
        yAxis: [
            {
                title: false
            }, {
                title: false,
                opposite: true
            }
        ],
        series: [
            {
                yAxis: 0
            },
            {
                yAxis: 1
            }
        ]
    });