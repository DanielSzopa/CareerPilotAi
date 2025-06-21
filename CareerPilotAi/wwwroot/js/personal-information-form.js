/**
 * Personal Information Form Handler
 * Manages dynamic form sections for resume data entry
 */
const PersonalInfoForm = {
    // Counters for array indices
    counters: {
        links: 0,
        skills: 0,
        experience: 0,
        education: 0,
        projects: 0,
        certifications: 0,
        languages: 0,
        otherInfo: 0
    },

    // Maximum limits
    limits: {
        links: 5,
        skills: 30,
        experience: 10,
        education: 5,
        projects: 10,
        certifications: 10,
        languages: 10,
        otherInfo: 5
    },

    init() {
        this.initializeExistingItems();
        this.bindEvents();
    },

    initializeExistingItems() {
        // Initialize counters based on existing items
        this.counters.links = $('#linksContainer .dynamic-item').length;
        this.counters.skills = $('#skillsContainer .dynamic-item').length;
        this.counters.experience = $('#experienceContainer .dynamic-item').length;
        this.counters.education = $('#educationContainer .dynamic-item').length;
        this.counters.projects = $('#projectsContainer .dynamic-item').length;
        this.counters.certifications = $('#certificationsContainer .dynamic-item').length;
        this.counters.languages = $('#languagesContainer .dynamic-item').length;
        this.counters.otherInfo = $('#otherInfoContainer .dynamic-item').length;
    },

    bindEvents() {
        // Bind remove events for existing items
        $(document).on('click', '.remove-btn', function() {
            const section = $(this).data('section');
            const index = $(this).data('index');
            PersonalInfoForm.removeItem(section, index);
        });

        // Bind add content point events
        $(document).on('click', '.add-content-point', function() {
            const container = $(this).siblings('.content-points-container');
            PersonalInfoForm.addContentPoint(container);
        });

        // Bind remove content point events
        $(document).on('click', '.remove-content-point', function() {
            $(this).parent().remove();
        });

        // Bind add technology events
        $(document).on('click', '.add-technology', function() {
            const container = $(this).siblings('.technologies-container');
            PersonalInfoForm.addTechnology(container);
        });

        // Bind remove technology events
        $(document).on('click', '.remove-technology', function() {
            $(this).parent().remove();
        });

        // Bind present checkbox events
        $(document).on('change', '.present-checkbox', function() {
            const endDateInput = $(this).closest('.dynamic-item').find('.end-date-input');
            if ($(this).is(':checked')) {
                endDateInput.prop('disabled', true).val('');
            } else {
                endDateInput.prop('disabled', false);
            }
        });

        // Bind real-time validation events
        $(document).on('input keyup change', '.skill-input', function() {
            PersonalInfoForm.validateSkillsRealTime();
        });

        $(document).on('input keyup change', '.education-input', function() {
            PersonalInfoForm.validateEducationRealTime();
        });

        // Bind skill level validation
        $(document).on('change', 'select[name*="Skills"][name*="Level"]', function() {
            PersonalInfoForm.validateSkillLevelRealTime($(this));
        });

        // Bind language proficiency validation
        $(document).on('change', 'select[name*="Languages"][name*="Proficiency"]', function() {
            PersonalInfoForm.validateLanguageProficiencyRealTime($(this));
        });

        // Bind email validation
        $(document).on('input keyup', 'input[name="PersonalInformation.Email"]', function() {
            PersonalInfoForm.validateEmailRealTime();
        });

        // Bind full name validation
        $(document).on('input keyup', 'input[name="PersonalInformation.Fullname"]', function() {
            PersonalInfoForm.validateFullNameRealTime();
        });
    },

    addLink() {
        if (this.counters.links >= this.limits.links) {
            alert(`You can add maximum ${this.limits.links} links.`);
            return;
        }

        const index = this.counters.links;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="links" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label class="form-label">Label *</label>
                            <input name="PersonalInformation.Links[${index}].Label" class="form-control" placeholder="e.g., LinkedIn, GitHub" required />
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="form-group mb-3">
                            <label class="form-label">URL *</label>
                            <input name="PersonalInformation.Links[${index}].Url" class="form-control" type="text" placeholder="e.g., github.com/username, linkedin.com/in/username" required />
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        $('#linksContainer').append(html);
        this.counters.links++;
        this.updateAddButtonState('links');
    },

    addSkill() {
        if (this.counters.skills >= this.limits.skills) {
            alert(`You can add maximum ${this.limits.skills} skills.`);
            return;
        }

        const index = this.counters.skills;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="skills" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group mb-3">
                            <label class="form-label">Skill Name *</label>
                            <input name="Skills[${index}].Label" class="form-control skill-input" placeholder="e.g., JavaScript, Project Management" required />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label class="form-label">Level</label>
                            <select name="Skills[${index}].Level" class="form-control">
                                <option value="n/a" selected>n/a</option>
                                <option value="Junior">Junior</option>
                                <option value="Regular">Regular</option>
                                <option value="Expert">Expert</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        $('#skillsContainer').append(html);
        this.counters.skills++;
        this.updateAddButtonState('skills');
        
        // Trigger validation immediately after adding
        setTimeout(() => {
            this.validateSkillsRealTime();
        }, 100);
    },

    addExperience() {
        if (this.counters.experience >= this.limits.experience) {
            alert(`You can add maximum ${this.limits.experience} work experiences.`);
            return;
        }

        const index = this.counters.experience;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="experience" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Job Title *</label>
                            <input name="Experience[${index}].Title" class="form-control" required />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Company *</label>
                            <input name="Experience[${index}].Company" class="form-control" required />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label class="form-label">Location</label>
                            <input name="Experience[${index}].Location" class="form-control" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label class="form-label">Start Date</label>
                            <input name="Experience[${index}].StartDate" class="form-control" type="date" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label class="form-label">End Date</label>
                            <input name="Experience[${index}].EndDate" class="form-control end-date-input" type="date" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group mb-3">
                            <div class="form-check mt-4">
                                <input name="Experience[${index}].Present" class="form-check-input present-checkbox" type="checkbox" />
                                <label class="form-check-label">Current</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label class="form-label">Job Responsibilities</label>
                    <div class="content-points-container" data-prefix="Experience[${index}].ListOfContentPoints">
                        <!-- Content points will be added here -->
                    </div>
                    <button type="button" class="btn btn-sm btn-outline-secondary add-content-point">
                        <i class="fas fa-plus"></i> Add Responsibility
                    </button>
                </div>
            </div>
        `;
        
        $('#experienceContainer').append(html);
        this.counters.experience++;
        this.updateAddButtonState('experience');
    },

    addEducation() {
        if (this.counters.education >= this.limits.education) {
            alert(`You can add maximum ${this.limits.education} education entries.`);
            return;
        }

        const index = this.counters.education;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="education" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-lg-6 col-md-12">
                        <div class="form-group mb-3">
                            <label class="form-label">Degree *</label>
                            <input name="Education[${index}].Degree" class="form-control education-input" required />
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-12">
                        <div class="form-group mb-3">
                            <label class="form-label">Institution *</label>
                            <input name="Education[${index}].Institution" class="form-control education-input" required />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6 col-md-12">
                        <div class="form-group mb-3">
                            <label class="form-label">Location</label>
                            <input name="Education[${index}].Location" class="form-control" />
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-12">
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group mb-3">
                                    <label class="form-label">Start Date</label>
                                    <input name="Education[${index}].StartDate" class="form-control" type="date" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group mb-3">
                                    <label class="form-label">End Date</label>
                                    <input name="Education[${index}].EndDate" class="form-control end-date-input" type="date" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-check">
                            <input name="Education[${index}].Present" class="form-check-input present-checkbox" type="checkbox" />
                            <label class="form-check-label">Currently studying here</label>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        $('#educationContainer').append(html);
        this.counters.education++;
        this.updateAddButtonState('education');
        this.validateEducationRealTime();
    },

    addProject() {
        if (this.counters.projects >= this.limits.projects) {
            alert(`You can add maximum ${this.limits.projects} projects.`);
            return;
        }

        const index = this.counters.projects;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="projects" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-8">
                        <div class="form-group mb-3">
                            <label class="form-label">Project Title *</label>
                            <input name="Projects[${index}].Title" class="form-control" required />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label class="form-label">Project Link</label>
                            <input name="Projects[${index}].Link" class="form-control" type="url" placeholder="https://..." />
                        </div>
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label class="form-label">Project Description</label>
                    <div class="content-points-container" data-prefix="Projects[${index}].ListOfContentPoints">
                        <!-- Content points will be added here -->
                    </div>
                    <button type="button" class="btn btn-sm btn-outline-secondary add-content-point">
                        <i class="fas fa-plus"></i> Add Description Point
                    </button>
                </div>
                <div class="form-group mb-3">
                    <label class="form-label">Technologies Used</label>
                    <div class="technologies-container" data-prefix="Projects[${index}].Technologies">
                        <!-- Technologies will be added here -->
                    </div>
                    <button type="button" class="btn btn-sm btn-outline-secondary add-technology">
                        <i class="fas fa-plus"></i> Add Technology
                    </button>
                </div>
            </div>
        `;
        
        $('#projectsContainer').append(html);
        this.counters.projects++;
        this.updateAddButtonState('projects');
    },

    addCertification() {
        if (this.counters.certifications >= this.limits.certifications) {
            alert(`You can add maximum ${this.limits.certifications} certifications.`);
            return;
        }

        const index = this.counters.certifications;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="certifications" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Certification Title *</label>
                            <input name="Certifications[${index}].Title" class="form-control" required />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Issuer *</label>
                            <input name="Certifications[${index}].Issuer" class="form-control" required />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label class="form-label">Date Obtained</label>
                            <input name="Certifications[${index}].Date" class="form-control" type="date" />
                        </div>
                    </div>
                    <div class="col-md-8">
                        <div class="form-group mb-3">
                            <label class="form-label">Certification Link</label>
                            <input name="Certifications[${index}].Link" class="form-control" type="url" placeholder="https://..." />
                        </div>
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label class="form-label">Description</label>
                    <textarea name="Certifications[${index}].Description" class="form-control" rows="3" placeholder="Brief description of the certification..."></textarea>
                </div>
            </div>
        `;
        
        $('#certificationsContainer').append(html);
        this.counters.certifications++;
        this.updateAddButtonState('certifications');
    },

    addLanguage() {
        if (this.counters.languages >= this.limits.languages) {
            alert(`You can add maximum ${this.limits.languages} languages.`);
            return;
        }

        const index = this.counters.languages;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="languages" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Language *</label>
                            <input name="Languages[${index}].Language" class="form-control" required />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Proficiency Level *</label>
                            <select name="Languages[${index}].Proficiency" class="form-control" required>
                                <option value="">Select Proficiency</option>
                                <option value="Native">Native</option>
                                <option value="Fluent">Fluent</option>
                                <option value="Advanced">Advanced</option>
                                <option value="Intermediate">Intermediate</option>
                                <option value="Basic">Basic</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        $('#languagesContainer').append(html);
        this.counters.languages++;
        this.updateAddButtonState('languages');
    },

    addOtherInformation() {
        if (this.counters.otherInfo >= this.limits.otherInfo) {
            alert(`You can add maximum ${this.limits.otherInfo} additional information sections.`);
            return;
        }

        const index = this.counters.otherInfo;
        const html = `
            <div class="dynamic-item" data-index="${index}">
                <button type="button" class="btn btn-sm btn-outline-danger remove-btn" data-section="otherInfo" data-index="${index}">
                    <i class="fas fa-times"></i>
                </button>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Label *</label>
                            <input name="OtherInformation[${index}].Label" class="form-control" placeholder="e.g., Hobbies, Interests, Awards" required />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group mb-3">
                            <label class="form-label">Title</label>
                            <input name="OtherInformation[${index}].Title" class="form-control" placeholder="Optional subtitle" />
                        </div>
                    </div>
                </div>
                <div class="form-group mb-3">
                    <label class="form-label">Details</label>
                    <div class="content-points-container" data-prefix="OtherInformation[${index}].ListOfContentPoints">
                        <!-- Content points will be added here -->
                    </div>
                    <button type="button" class="btn btn-sm btn-outline-secondary add-content-point">
                        <i class="fas fa-plus"></i> Add Detail
                    </button>
                </div>
            </div>
        `;
        
        $('#otherInfoContainer').append(html);
        this.counters.otherInfo++;
        this.updateAddButtonState('otherInfo');
    },

    addContentPoint(container) {
        const prefix = container.data('prefix');
        const pointIndex = container.children().length;
        
        const html = `
            <div class="content-point-item">
                <input name="${prefix}[${pointIndex}]" class="form-control" placeholder="Enter description..." />
                <button type="button" class="btn btn-sm btn-outline-danger remove-content-point">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;
        
        container.append(html);
    },

    addTechnology(container) {
        const prefix = container.data('prefix');
        const techIndex = container.children().length;
        
        const html = `
            <div class="content-point-item">
                <input name="${prefix}[${techIndex}]" class="form-control" placeholder="Enter technology..." />
                <button type="button" class="btn btn-sm btn-outline-danger remove-technology">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        `;
        
        container.append(html);
    },

    removeItem(section, index) {
        $(`[data-section="${section}"][data-index="${index}"]`).closest('.dynamic-item').remove();
        this.counters[section]--;
        this.updateAddButtonState(section);
        this.reindexItems(section);
        
        // Update real-time validation after removing items
        if (section === 'skills') {
            this.validateSkillsRealTime();
        } else if (section === 'education') {
            this.validateEducationRealTime();
        }
    },

    reindexItems(section) {
        const containerMap = {
            links: '#linksContainer',
            skills: '#skillsContainer',
            experience: '#experienceContainer',
            education: '#educationContainer',
            projects: '#projectsContainer',
            certifications: '#certificationsContainer',
            languages: '#languagesContainer',
            otherInfo: '#otherInfoContainer'
        };

        const container = $(containerMap[section]);
        container.find('.dynamic-item').each(function(newIndex) {
            const $item = $(this);
            $item.attr('data-index', newIndex);
            $item.find('.remove-btn').attr('data-index', newIndex);
            
            // Update all input names within this item
            $item.find('input, select, textarea').each(function() {
                const $input = $(this);
                const name = $input.attr('name');
                if (name) {
                    const newName = name.replace(/\[\d+\]/, `[${newIndex}]`);
                    $input.attr('name', newName);
                }
            });
        });
    },

    updateAddButtonState(section) {
        const buttonMap = {
            links: '#addLinkBtn',
            skills: '#addSkillBtn'
        };

        const button = $(buttonMap[section]);
        if (button.length && this.counters[section] >= this.limits[section]) {
            button.prop('disabled', true);
        } else if (button.length) {
            button.prop('disabled', false);
        }
    },

    validateForm() {
        let isValid = true;
        let messages = [];

        // Validate required personal information fields
        const fullName = $('input[name="PersonalInformation.Fullname"]').val()?.trim();
        const email = $('input[name="PersonalInformation.Email"]').val()?.trim();

        if (!fullName || fullName.length < 2) {
            isValid = false;
            messages.push('Full name is required and must be at least 2 characters long.');
        }

        if (!email) {
            isValid = false;
            messages.push('Email address is required.');
        } else if (!this.isValidEmail(email)) {
            isValid = false;
            messages.push('Please enter a valid email address.');
        }

        // Validate minimum skills requirement
        const skillsCount = this.counters.skills;
        const validSkillsCount = this.getValidSkillsCount();
        
        if (validSkillsCount < 3) {
            isValid = false;
            messages.push('At least 3 skills are required.');
        }

        // Validate minimum education requirement
        const educationCount = this.counters.education;
        const validEducationCount = this.getValidEducationCount();
        
        if (validEducationCount < 1) {
            isValid = false;
            messages.push('At least one education entry is required.');
        }

        // Validate other required fields
        const requiredFields = $('#personalInfoForm input[required], #personalInfoForm select[required]');
        requiredFields.each(function() {
            if (!$(this).val()?.trim()) {
                isValid = false;
                if (messages.indexOf('Please fill in all required fields.') === -1) {
                    messages.push('Please fill in all required fields.');
                }
                return false;
            }
        });

        // Check limits
        for (const [section, count] of Object.entries(this.counters)) {
            if (count > this.limits[section]) {
                isValid = false;
                messages.push(`Too many items in ${section} section. Maximum allowed: ${this.limits[section]}`);
                break;
            }
        }

        // Validate skill levels
        const invalidSkillLevels = this.getInvalidSkillLevels();
        if (invalidSkillLevels.length > 0) {
            isValid = false;
            messages.push('Some skills have invalid levels. Please select n/a, Junior, Regular, or Expert.');
        }

        // Validate language proficiencies
        const invalidLanguageProficiencies = this.getInvalidLanguageProficiencies();
        if (invalidLanguageProficiencies.length > 0) {
            isValid = false;
            messages.push('Some languages have invalid proficiency levels. Please select Native, Fluent, Advanced, Intermediate, or Basic.');
        }

        const message = messages.length > 0 ? messages.join('\n') : '';
        return { isValid, message };
    },

    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    },

    getValidSkillsCount() {
        let count = 0;
        $('#skillsContainer .dynamic-item').each(function() {
            const skillName = $(this).find('input[name*="Skills"][name*="Label"]').val()?.trim();
            if (skillName && skillName.length >= 2) {
                count++;
            }
        });
        return count;
    },

    getValidEducationCount() {
        let count = 0;
        $('#educationContainer .dynamic-item').each(function() {
            const degree = $(this).find('input[name*="Degree"]').val()?.trim();
            const institution = $(this).find('input[name*="Institution"]').val()?.trim();
            if (degree && institution) {
                count++;
            }
        });
        return count;
    },

    validateSkillsRealTime() {
        const validSkillsCount = this.getValidSkillsCount();
        const totalSkillsCount = this.counters.skills;
        const messageContainer = $('#skillsValidationMessage');
        
        if (validSkillsCount < 3) {
            const remaining = 3 - validSkillsCount;
            messageContainer.text(`${validSkillsCount}/3 skills added. Please add ${remaining} more skill(s).`);
            messageContainer.show();
        } else {
            messageContainer.hide();
        }
        
        // Debug info - remove this later if needed
        console.log(`Total skills: ${totalSkillsCount}, Valid skills: ${validSkillsCount}`);
    },

    validateEducationRealTime() {
        const validEducationCount = this.getValidEducationCount();
        const messageContainer = $('#educationValidationMessage');
        
        if (validEducationCount < 1) {
            messageContainer.text('At least one education entry with both degree and institution is required.');
            messageContainer.show();
        } else {
            messageContainer.hide();
        }
    },

    validateEmailRealTime() {
        const email = $('input[name="PersonalInformation.Email"]').val()?.trim();
        const emailField = $('input[name="PersonalInformation.Email"]');
        
        // Hide any existing server-side validation messages
        emailField.siblings('.text-danger').hide();
        
        if (email && !this.isValidEmail(email)) {
            emailField.addClass('is-invalid');
            this.showFieldError(emailField, 'Please enter a valid email address.');
        } else {
            emailField.removeClass('is-invalid');
            this.hideFieldError(emailField);
            // Show server-side validation if it exists
            emailField.siblings('.text-danger').show();
        }
    },

    validateFullNameRealTime() {
        const fullName = $('input[name="PersonalInformation.Fullname"]').val()?.trim();
        const fullNameField = $('input[name="PersonalInformation.Fullname"]');
        
        // Hide any existing server-side validation messages
        fullNameField.siblings('.text-danger').hide();
        
        if (fullName && fullName.length < 2) {
            fullNameField.addClass('is-invalid');
            this.showFieldError(fullNameField, 'Full name must be between 2 and 100 characters.');
        } else if (fullName && fullName.length > 100) {
            fullNameField.addClass('is-invalid');
            this.showFieldError(fullNameField, 'Full name must be between 2 and 100 characters.');
        } else {
            fullNameField.removeClass('is-invalid');
            this.hideFieldError(fullNameField);
            // Show server-side validation if it exists
            fullNameField.siblings('.text-danger').show();
        }
    },

    showFieldError(field, message) {
        // Remove any existing client-side error messages first
        this.hideFieldError(field);
        
        // Create new error message
        const errorElement = $(`<div class="field-error text-danger small mt-1">${message}</div>`);
        field.after(errorElement);
    },

    hideFieldError(field) {
        field.siblings('.field-error').remove();
    },

    validateSkillLevelRealTime(select) {
        const level = select.val();
        const validLevels = ["n/a", "Junior", "Regular", "Expert"];
        
        // Clear any existing validation styling
        select.removeClass('is-invalid');
        this.hideFieldError(select);
        
        if (level && !validLevels.includes(level)) {
            select.addClass('is-invalid');
            this.showFieldError(select, 'Skill level must be n/a, Junior, Regular, or Expert.');
        }
    },

    validateLanguageProficiencyRealTime(select) {
        const proficiency = select.val();
        const validProficiencies = ["Native", "Fluent", "Advanced", "Intermediate", "Basic"];
        
        // Clear any existing validation styling
        select.removeClass('is-invalid');
        this.hideFieldError(select);
        
        if (proficiency && !validProficiencies.includes(proficiency)) {
            select.addClass('is-invalid');
            this.showFieldError(select, 'Language proficiency must be Native, Fluent, Advanced, Intermediate, or Basic.');
        }
    },

    getInvalidSkillLevels() {
        const invalidLevels = [];
        $('#skillsContainer .dynamic-item').each(function() {
            const level = $(this).find('select[name*="Skills"][name*="Level"]').val();
            if (level && !["n/a", "Junior", "Regular", "Expert"].includes(level)) {
                invalidLevels.push($(this).find('input[name*="Skills"][name*="Label"]').val());
            }
        });
        return invalidLevels;
    },

    getInvalidLanguageProficiencies() {
        const invalidProficiencies = [];
        $('#languagesContainer .dynamic-item').each(function() {
            const proficiency = $(this).find('select[name*="Languages"][name*="Proficiency"]').val();
            if (proficiency && !["Native", "Fluent", "Advanced", "Intermediate", "Basic"].includes(proficiency)) {
                invalidProficiencies.push($(this).find('input[name*="Languages"][name*="Language"]').val());
            }
        });
        return invalidProficiencies;
    }
};

// Global functions for onclick handlers
function addLink() { PersonalInfoForm.addLink(); }
function addSkill() { PersonalInfoForm.addSkill(); }
function addExperience() { PersonalInfoForm.addExperience(); }
function addEducation() { PersonalInfoForm.addEducation(); }
function addProject() { PersonalInfoForm.addProject(); }
function addCertification() { PersonalInfoForm.addCertification(); }
function addLanguage() { PersonalInfoForm.addLanguage(); }
function addOtherInformation() { PersonalInfoForm.addOtherInformation(); } 