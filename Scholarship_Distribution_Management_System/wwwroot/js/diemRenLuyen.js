/**
 * Điểm Rèn Luyện JavaScript
 * Handles datatable initialization and other interactive features
 */

document.addEventListener('DOMContentLoaded', function() {
    // Initialize DataTable if it exists on the page
    if (document.getElementById('diemRenLuyenTable')) {
        initializeDiemRenLuyenTable();
    }
    
    // Initialize export button interactions
    const exportBtn = document.querySelector('a[asp-action="ExportToExcel"]');
    if (exportBtn) {
        initializeExportButton(exportBtn);
    }
});

/**
 * Initialize the DataTable for discipline scores
 */
function initializeDiemRenLuyenTable() {
    // Initialize DataTable with custom configuration
    const table = new DataTable('#diemRenLuyenTable', {
        responsive: true,
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Vietnamese.json"
        },        columnDefs: [
            // Apply consistent alignment
            { className: "text-center", targets: [0, 5, 6] }, // Center align STT, điểm rèn luyện, and actions columns
            { orderable: false, targets: 6 } // Disable sorting on action column
        ],
        // Custom layout configuration - search on left, items per page and pagination tightly on right
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
 * Initialize interactions for the export button
 */
function initializeExportButton(exportBtn) {
    // Add loading state when clicked
    exportBtn.addEventListener('click', function() {
        const icon = this.querySelector('i');
        const originalContent = this.innerHTML;
        
        // Show loading state
        this.innerHTML = '<i class="bi bi-hourglass-split"></i> Đang xuất...';
        this.classList.add('disabled');
        
        // Restore original state after export completes or fails
        setTimeout(function() {
            exportBtn.innerHTML = originalContent;
            exportBtn.classList.remove('disabled');
        }, 3000); // Allow 3 seconds for the export to complete
    });
    
    // Add tooltip to the export button
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        new bootstrap.Tooltip(exportBtn, {
            title: 'Xuất danh sách điểm rèn luyện ra file Excel',
            placement: 'bottom'
        });
    }
}
