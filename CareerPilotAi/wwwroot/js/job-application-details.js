$(document).ready(function() {         

    const jobApplicationId = document.getElementById('jobApplicationId').textContent;
    const interviewQuestionsManager = new InterviewQuestionsManager({
        jobApplicationId: jobApplicationId,
        showSuccessMessage: showSuccessMessage,
        showErrorMessage: showErrorMessage
    });

    const TabManager = {
        defaultTab: 'job-details',
        paramName: 'tab',
        
        tabConfigs: {
            'job-details': {
                tabSelector: '#job-details-tab',
                contentSelector: '#job-details',
                onActivate: function() {}
            },
            'interview-questions': {
                tabSelector: '#interview-questions-tab',
                contentSelector: '#interview-questions',
                onActivate: function() {
                    interviewQuestionsManager.onTabActivate();
                }
            }
        },
        
        init: function() {
            this.bindEvents();
            this.activateTabFromUrl();
        },
        
        bindEvents: function() {
            const self = this;
            
            $('#jobApplicationTabs button[data-bs-toggle="tab"]').on('click', function(e) {
                e.preventDefault();
                
                const tabParam = $(this).data('tab-param');
                if (tabParam) {
                    self.updateUrlParameter(tabParam);
                    self.activateTab(tabParam);
                }
                interviewQuestionsManager.clearTabMessages();
            });
            
            $(window).on('popstate', function() {
                self.activateTabFromUrl();
            });
            
            $('#jobApplicationTabs button[data-bs-toggle="tab"]').on('shown.bs.tab', function(e) {
                const tabParam = $(e.target).data('tab-param');
                const config = self.tabConfigs[tabParam];
                if (config && config.onActivate) {
                    config.onActivate();
                }
            });
        },
        
        getUrlParams: function() {
            return new URLSearchParams(window.location.search);
        },
        
        getCurrentTabFromUrl: function() {
            return this.getUrlParams().get(this.paramName);
        },
        
        updateUrlParameter: function(tabParam) {
            const url = new URL(window.location);
            
            if (tabParam === this.defaultTab) {
                url.searchParams.delete(this.paramName);
            } else {
                url.searchParams.set(this.paramName, tabParam);
            }
            
            window.history.pushState({ tab: tabParam }, '', url);
        },
        
        activateTabFromUrl: function() {
            const currentTab = this.getCurrentTabFromUrl();
            const targetTab = currentTab && this.tabConfigs[currentTab] ? currentTab : this.defaultTab;
            
            this.activateTab(targetTab);
        },
        
        activateTab: function(tabParam) {
            const config = this.tabConfigs[tabParam];
            if (!config) {
                console.warn(`Tab configuration not found for: ${tabParam}`);
                return;
            }
            
            $('#jobApplicationTabs .nav-link').removeClass('active');
            $('.tab-pane').removeClass('show active');
            
            $(config.tabSelector).addClass('active');
            $(config.contentSelector).addClass('show active');
            
            $(config.tabSelector).trigger('shown.bs.tab');
        },
        
        addTab: function(tabParam, config) {
            this.tabConfigs[tabParam] = config;
        },
        
        navigateToTab: function(tabParam) {
            if (this.tabConfigs[tabParam]) {
                this.updateUrlParameter(tabParam);
                this.activateTab(tabParam);
            }
        }
    };
    
    TabManager.init();
    
    $('.dropdown-menu .dropdown-item').on('click', function(e) {
        e.preventDefault();
        
        const newStatus = $(this).data('status');
        const newColorClass = $(this).data('color-class');
        
        const $statusButton = $('#statusDropdown');
        const $statusDot = $statusButton.find('.status-dot');
        const $statusText = $statusButton.find('.status-text');
        
        $statusDot.removeClass('status-draft status-submitted status-interview-scheduled status-waiting-for-offer status-received-offer status-rejected status-no-contact');
        $statusDot.addClass(newColorClass);
        $statusText.text(newStatus);
        
        updateApplicationStatus(jobApplicationId, newStatus);
    });
    
    async function updateApplicationStatus(jobApplicationId, newStatus) {
        try {
            const response = await fetch(`/job-applications/api/status/${jobApplicationId}`, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    Status: newStatus
                })
            });

            if (!response.ok) {
                const errorData = await response.json();
                const errorMessage = errorData.detail || errorData.error || 'Failed to update application status. Please try again.';
                throw new Error(errorMessage);
            }

            const data = await response.json();
            showSuccessMessage(`Application status updated to "${newStatus}" successfully!`);
        } catch (error) {
            console.error('Error updating application status:', error);
            showErrorMessage(error.message);
            location.reload();
        }
    }
    
    interviewQuestionsManager.init();
    
    if (typeof bootstrap !== 'undefined') {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    function showSuccessMessage(message) {
        const successHtml = `
            <div class="alert alert-success alert-dismissible fade show mt-2" role="alert">
                <i class="fas fa-check-circle me-2"></i>${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        const $jobDetailsDiv = $('#job-details');
        if ($jobDetailsDiv.length) {
            $jobDetailsDiv.prepend(successHtml);
        } else {
            $('body').prepend(successHtml);
        }
        
        setTimeout(() => {
            $('.alert-success').fadeOut();
        }, 8000);
    }
    
    function showErrorMessage(message) {
        const errorHtml = `
            <div class="alert alert-danger alert-dismissible fade show mt-2" role="alert">
                <i class="fas fa-exclamation-triangle me-2"></i>${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        const $jobDetailsDiv = $('#job-details');
        if ($jobDetailsDiv.length) {
            $jobDetailsDiv.prepend(errorHtml);
        } else {
            $('body').prepend(errorHtml);
        }
        
        setTimeout(() => {
            $('.alert-danger').fadeOut();
        }, 8000);
    }
});

function confirmDelete(jobApplicationId, jobTitle) {
    if (confirm(`Are you sure you want to delete the job application for "${jobTitle}"? This action cannot be undone.`)) {
        deleteJobApplication(jobApplicationId);
    }
}

async function deleteJobApplication(jobApplicationId) {
    try {
        const response = await fetch(`/job-applications/api/delete/${jobApplicationId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        });

        if (response.ok) {
            const result = await response.json();
            alert(result.message);
            window.location.href = '/job-applications';
        } else {
            const errorData = await response.json();
            alert(errorData.detail || 'An error occurred while deleting the job application.');
        }
    } catch (error) {
        console.error('Error deleting job application:', error);
        alert('An error occurred while deleting the job application. Please try again.');
    }
}