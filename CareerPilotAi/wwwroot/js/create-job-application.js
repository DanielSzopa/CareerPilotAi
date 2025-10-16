/**
 * Create Job Application - Main JavaScript
 * Handles AI parsing, skills management, form validation, and word counting
 */

$(document).ready(function() {
    // ===========================================
    // 1. WORD COUNTERS INITIALIZATION
    // ===========================================
    
    // AI Parsing textarea word counter
    const aiParsingWordCounter = new WordCounter({
        textAreaSelector: '#aiParsingTextarea',
        wordCountSelector: '#aiParsingWordCount',
        maxWords: 5000,
        warningThreshold: 4000,
        submitButtonSelector: '#parseWithAiButton',
        errorMessageSelector: '#aiParsingWordLimitError'
    });
    
    // Job Description field word counter
    const jobDescWordCounter = new WordCounter({
        textAreaSelector: '#jobDescriptionField',
        wordCountSelector: '#jobDescWordCount',
        maxWords: 5000,
        warningThreshold: 4000,
        submitButtonSelector: null, // Don't disable form submit, just show warning
        errorMessageSelector: null
    });
    
    // ===========================================
    // 2. SKILLS MANAGEMENT
    // ===========================================
    
    let skills = [];
    let skillIndex = 0;
    
    function addSkill() {
        const name = $('#skillNameInput').val().trim();
        const level = $('#skillLevelInput').val();
        
        // Validation
        if (!name) {
            showAlert('Please enter a skill name');
            return;
        }
        
        if (!level) {
            showAlert('Please select a skill level');
            return;
        }
        
        if (name.length < 2) {
            showAlert('Skill name must be at least 2 characters');
            return;
        }
        
        if (name.length > 50) {
            showAlert('Skill name cannot exceed 50 characters');
            return;
        }
        
        if (skills.length >= 20) {
            showAlert('Maximum 20 skills allowed');
            return;
        }
        
        // Check for duplicates
        if (skills.some(s => s.name.toLowerCase() === name.toLowerCase())) {
            showAlert('This skill is already added');
            return;
        }
        
        // Add skill
        const skill = {
            id: skillIndex++,
            name: name,
            level: level
        };
        
        skills.push(skill);
        renderSkill(skill);
        updateSkillsCount();
        
        // Clear inputs
        $('#skillNameInput').val('');
        $('#skillLevelInput').val('');
        $('#skillNameInput').focus();
    }
    
    function removeSkill(skillId) {
        skills = skills.filter(s => s.id !== skillId);
        $(`#skill-${skillId}`).remove();
        $(`#skill-hidden-${skillId}-name`).remove();
        $(`#skill-hidden-${skillId}-level`).remove();
        updateSkillsCount();
    }
    
    function renderSkill(skill) {
        const levelDisplay = skill.level.replace(/([A-Z])/g, ' $1').trim();
        
        const skillHtml = `
            <span class="badge bg-primary me-2 mb-2" id="skill-${skill.id}" style="font-size: 0.95rem; padding: 0.5rem 0.75rem;">
                ${escapeHtml(skill.name)} 
                <span class="badge bg-light text-dark ms-1">${levelDisplay}</span>
                <button type="button" class="btn-close btn-close-white ms-2" style="font-size: 0.7rem;" onclick="removeSkillById(${skill.id})"></button>
            </span>
        `;
        
        $('#skillsContainer').append(skillHtml);
        
        // Add hidden fields for form submission
        const hiddenFieldsHtml = `
            <input type="hidden" name="Skills[${skills.length - 1}].Name" value="${escapeHtml(skill.name)}" id="skill-hidden-${skill.id}-name" />
            <input type="hidden" name="Skills[${skills.length - 1}].Level" value="${skill.level}" id="skill-hidden-${skill.id}-level" />
        `;
        
        $('#skillsHiddenFields').append(hiddenFieldsHtml);
    }
    
    function updateSkillsCount() {
        $('#skillsCount').text(skills.length);
    }
    
    // Make removeSkill globally accessible
    window.removeSkillById = removeSkill;
    
    // Event handlers for skills
    $('#addSkillButton').on('click', addSkill);
    
    $('#skillNameInput').on('keypress', function(e) {
        if (e.which === 13) { // Enter key
            e.preventDefault();
            addSkill();
        }
    });
    
    // ===========================================
    // 3. AI PARSING FUNCTIONALITY
    // ===========================================
    
    $('#parseWithAiButton').on('click', function(e) {
        e.preventDefault();
        
        const $button = $(this);
        const jobDescriptionText = $('#aiParsingTextarea').val().trim();
        
        // Clear previous alerts
        $('#aiParsingAlertContainer').empty();
        
        // Validation
        if (!jobDescriptionText) {
            showAiParsingAlert('Please enter job description text before parsing.', 'warning');
            return;
        }
        
        const validation = aiParsingWordCounter.validateWordCount();
        if (!validation.isValid) {
            showAiParsingAlert('Job description exceeds 5,000 words limit. Please shorten the text.', 'danger');
            return;
        }
        
        // Show loading state
        showLoadingState($button);
        
        // Create AbortController for timeout
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 40000); // 40 seconds
        
        // Make AJAX request
        fetch('/job-applications/api/parse-job-description', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            body: JSON.stringify({
                JobDescriptionText: jobDescriptionText
            }),
            signal: controller.signal
        })
        .then(response => {
            clearTimeout(timeoutId);
            return response.json();
        })
        .then(data => {
            if (data.success && data.data) {
                populateFormWithParsedData(data.data, data.parsingResult, data.missingFields);
                
                // Show success message
                let message = 'Job description parsed successfully!';
                if (data.parsingResult === 1) { // PartialSuccess
                    message += ` Note: Some fields could not be extracted: ${data.missingFields.join(', ')}`;
                }
                showAiParsingAlert(message, data.parsingResult === 0 ? 'success' : 'info');
                
                // Collapse the AI parsing accordion
                const collapseElement = document.getElementById('collapseAiParsing');
                const bsCollapse = new bootstrap.Collapse(collapseElement, {
                    toggle: false
                });
                bsCollapse.hide();
                
                // Scroll to form
                $('html, body').animate({
                    scrollTop: $('#createJobApplicationForm').offset().top - 20
                }, 500);
            } else {
                showAiParsingAlert('Failed to parse job description. Please fill the form manually.', 'danger');
            }
        })
        .catch(error => {
            clearTimeout(timeoutId);
            if (error.name === 'AbortError') {
                showAiParsingAlert('Request timed out. Please try again or fill the form manually.', 'danger');
            } else {
                showAiParsingAlert('An error occurred while parsing. Please try again or fill the form manually.', 'danger');
            }
            console.error('AI Parsing error:', error);
        })
        .finally(() => {
            hideLoadingState($button);
        });
    });
    
    function populateFormWithParsedData(data, parsingResult, missingFields) {
        // Populate basic information
        if (data.companyName) $('#CompanyName').val(data.companyName);
        if (data.position) $('#Position').val(data.position);
        if (data.jobDescription) {
            $('#jobDescriptionField').val(data.jobDescription);
            $('#jobDescriptionField').trigger('input'); // Update word counter
        }
        
        // Populate job details
        if (data.experienceLevel) $('#ExperienceLevel').val(data.experienceLevel);
        if (data.location) $('#Location').val(data.location);
        if (data.workMode) $('#WorkMode').val(data.workMode);
        if (data.contractType) $('#ContractType').val(data.contractType);
        
        // Populate salary information
        if (data.salaryMin) $('#SalaryMin').val(data.salaryMin);
        if (data.salaryMax) $('#SalaryMax').val(data.salaryMax);
        if (data.salaryType) $('#SalaryType').val(data.salaryType);
        if (data.salaryPeriod) $('#SalaryPeriod').val(data.salaryPeriod);
        
        // Populate additional information
        if (data.jobUrl) $('#JobUrl').val(data.jobUrl);
        if (data.status) $('#Status').val(data.status);
        
        // Populate skills
        if (data.skills && data.skills.length > 0) {
            // Clear existing skills
            skills = [];
            skillIndex = 0;
            $('#skillsContainer').empty();
            $('#skillsHiddenFields').empty();
            
            // Add parsed skills
            data.skills.forEach(skill => {
                const skillObj = {
                    id: skillIndex++,
                    name: skill.name,
                    level: skill.level
                };
                skills.push(skillObj);
                renderSkill(skillObj);
            });
            
            updateSkillsCount();
        }
    }
    
    function showLoadingState($button) {
        $button.prop('disabled', true);
        $button.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Parsing...');
    }
    
    function hideLoadingState($button) {
        $button.prop('disabled', false);
        $button.html('<i class="fas fa-robot me-2"></i>Parse with AI');
    }
    
    function showAiParsingAlert(message, type) {
        const alertHtml = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${getAlertIcon(type)} ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        $('#aiParsingAlertContainer').html(alertHtml);
    }
    
    function getAlertIcon(type) {
        const icons = {
            'success': '<i class="fas fa-check-circle me-2"></i>',
            'info': '<i class="fas fa-info-circle me-2"></i>',
            'warning': '<i class="fas fa-exclamation-triangle me-2"></i>',
            'danger': '<i class="fas fa-exclamation-circle me-2"></i>'
        };
        return icons[type] || '';
    }
    
    // ===========================================
    // 4. FORM VALIDATION
    // ===========================================
    
    $('#createJobApplicationForm').on('submit', function(e) {
        // Perform client-side validation
        const jobDescValidation = jobDescWordCounter.validateWordCount();
        
        if (!jobDescValidation.isValid) {
            e.preventDefault();
            showAlert('Job description exceeds the word limit. Please shorten it.');
            return false;
        }
        
        if (jobDescValidation.wordCount < 50) {
            e.preventDefault();
            showAlert('Job description must contain at least 50 words.');
            return false;
        }
        
        // Validate salary if provided
        const salaryMin = parseFloat($('#SalaryMin').val()) || 0;
        const salaryMax = parseFloat($('#SalaryMax').val()) || 0;
        
        if (salaryMin && salaryMax && salaryMax < salaryMin) {
            e.preventDefault();
            showAlert('Maximum salary must be greater than or equal to minimum salary.');
            return false;
        }
        
        return true;
    });
    
    // ===========================================
    // 5. UTILITY FUNCTIONS
    // ===========================================
    
    function showAlert(message) {
        const alertHtml = `
            <div class="alert alert-danger alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        
        // Insert alert at the top of the form
        $('#skillsCardBody').prepend(alertHtml);

        // Auto-dismiss after 5 seconds
        setTimeout(function() {
            $('.alert').fadeOut('slow', function() {
                $(this).remove();
            });
        }, 5000);
    }
    
    function escapeHtml(text) {
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, function(m) { return map[m]; });
    }
});

