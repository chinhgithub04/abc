/**
 * Duyệt Điểm Rèn Luyện JavaScript
 * Handles datatable initialization and other interactive features
 */

document.addEventListener('DOMContentLoaded', function() {
    // Initialize DataTable if it exists on the page
    if (document.getElementById('duyetDiemTable')) {
        initializeDuyetDiemTable();
    }
    
    // Initialize tooltips
    initializeTooltips();
    
    // Initialize alert auto-dismissal
    initializeAlerts();
});

/**
 * Initialize the DataTable for discipline score approval
 */
function initializeDuyetDiemTable() {
    // Initialize DataTable with custom configuration
    const table = new DataTable('#duyetDiemTable', {
        responsive: true,
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Vietnamese.json"
        },
        columnDefs: [
            // Apply consistent alignment
            { className: "text-center", targets: [0, 3, 4, 5, 6, 7] }, // Center align specific columns
            { orderable: false, targets: 7 } // Disable sorting on action column
        ],
        // Custom layout configuration
        dom: '<"row mb-3"<"col-sm-6"f><"col-sm-6 text-end"l>>' +
             '<"row"<"col-sm-12"tr>>' +
             '<"row mt-3"<"col-sm-5"i><"col-sm-7 text-end"p>>',
        lengthMenu: [
            [10, 25, 50, -1],
            [10, 25, 50, "Tất cả"]
        ],
        // Configure search options
        search: {
            return: true,
        },
        // Custom styling for the info section
        initComplete: function() {
            const api = this.api();
            
            // Add a header for the search box
            $('.dataTables_filter').prepend('<label class="search-title">Tìm kiếm:</label>');
            
            // Style search input
            $('.dataTables_filter input')
                .attr('placeholder', 'Nhập từ khóa...')
                .addClass('form-control-sm');
                
            // Add tooltips to action buttons
            $('.btn-info').tooltip();
            
            // Move length menu after search
            $('.dataTables_length').addClass('text-end');
        }
    });
    
    // Add row numbers automatically when table is drawn
    table.on('draw', function() {
        table.column(0, {search:'applied', order:'applied'}).nodes().each(function(cell, i) {
            cell.innerHTML = i + 1;
        });
    });
}

/**
 * Initialize tooltips for interactive elements
 */
function initializeTooltips() {
    // Check if Bootstrap's tooltip function exists
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        // Initialize all tooltips
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }
}

/**
 * Initialize auto-dismissal for alert messages
 */
function initializeAlerts() {
    // Auto-dismiss alerts after 5 seconds
    setTimeout(function() {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(function(alert) {
            // Check if Bootstrap's alert function exists
            if (typeof bootstrap !== 'undefined' && bootstrap.Alert) {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            } else {
                // Fallback for when Bootstrap JS is not available
                alert.style.display = 'none';
            }
        });
    }, 5000);
}
