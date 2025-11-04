(function () {
    const RANGE_BUTTON_SELECTOR = '[data-dashboard-range]';

    function initializeDashboard(configuration) {
        if (!configuration) {
            return;
        }

        const statusCanvas = document.getElementById('statusChart');
        const trendCanvas = document.getElementById('applicationsTrendChart');

        if (!statusCanvas || !trendCanvas || !window.Chart) {
            return;
        }

        const statusChart = createStatusChart(statusCanvas, configuration.statusDistribution);
        const trendChart = createTrendChart(trendCanvas, configuration.thirtyDays);

        setupRangeToggle(trendChart, {
            30: configuration.thirtyDays,
            60: configuration.sixtyDays,
            90: configuration.ninetyDays
        });

        return { statusChart, trendChart };
    }

    function createStatusChart(canvas, statusDistribution) {
        const labels = statusDistribution.map(item => item.status);
        const data = statusDistribution.map(item => item.count);
        const colors = statusDistribution.map(item => item.color);
        const percentages = statusDistribution.map(item => item.percentage);

        return new Chart(canvas, {
            type: 'doughnut',
            data: {
                labels,
                datasets: [
                    {
                        data,
                        backgroundColor: colors,
                        hoverOffset: 12,
                        borderWidth: 2,
                        borderColor: '#fff'
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 16
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const label = context.label || '';
                                const value = context.raw ?? 0;
                                const index = context.dataIndex;
                                const percentage = percentages[index] ?? 0;
                                return `${label}: ${value} (${percentage.toFixed(1)}%)`;
                            }
                        }
                    }
                }
            }
        });
    }

    function createTrendChart(canvas, dataset) {
        return new Chart(canvas, {
            type: 'bar',
            data: {
                labels: dataset.labels,
                datasets: [
                    {
                        label: 'Applications',
                        data: dataset.values,
                        backgroundColor: 'rgba(13, 110, 253, 0.6)',
                        borderColor: '#0d6efd',
                        borderWidth: 1.5,
                        borderRadius: 6,
                        maxBarThickness: 36
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        ticks: {
                            maxRotation: 45,
                            minRotation: 0
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        beginAtZero: true,
                        ticks: {
                            precision: 0
                        }
                    }
                },
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const value = context.raw ?? 0;
                                return `Applications: ${value}`;
                            }
                        }
                    }
                }
            }
        });
    }

    function setupRangeToggle(trendChart, datasets) {
        const buttons = document.querySelectorAll(RANGE_BUTTON_SELECTOR);
        if (!buttons.length) {
            return;
        }

        buttons.forEach(button => {
            button.addEventListener('click', () => {
                const range = button.getAttribute('data-dashboard-range');
                if (!range || !datasets[range]) {
                    return;
                }

                updateActiveButton(buttons, button);
                updateTrendChart(trendChart, datasets[range]);
            });
        });
    }

    function updateActiveButton(buttons, activeButton) {
        buttons.forEach(button => button.classList.remove('active'));
        activeButton.classList.add('active');
    }

    function updateTrendChart(chart, dataset) {
        chart.data.labels = dataset.labels;
        chart.data.datasets[0].data = dataset.values;
        chart.update();
    }

    window.CareerPilotDashboard = {
        initializeDashboard
    };
})();

