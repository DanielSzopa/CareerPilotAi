/**
 * Interview Questions Manager
 * Handles all functionality related to the Interview Questions tab
 */
class InterviewQuestionsManager {
    constructor(options) {
        this.jobApplicationId = options.jobApplicationId;
        this.showSuccessMessage = options.showSuccessMessage;
        this.showErrorMessage = options.showErrorMessage;
        
        // Interview Questions specific variables
        this.originalInterviewPrepContent = '';
        this.interviewPrepWordCounter = null;
        this.interviewQuestions = [];
        this.questionsLoaded = false;
        this.MAX_QUESTIONS = 20;
        this.MAX_QUESTIONS_PER_BATCH = 10;
        
        // Deletion flag to prevent multiple simultaneous deletions
        this.isDeletingQuestion = false;
    }

    /**
     * Initialize the Interview Questions manager
     */
    init() {
        this.bindEvents();
        this.updateQuestionsCounter();
        this.initializeTooltips();
    }

    /**
     * Called when the Interview Questions tab is activated
     */
    onTabActivate() {
        if (!this.questionsLoaded) {
            this.fetchExistingInterviewQuestions();
        } else {
            // Update counter even if questions were already loaded
            this.updateQuestionsCounter();
        }
    }

    /**
     * Bind all event handlers for Interview Questions functionality
     */
    bindEvents() {
        // Unbind any existing events first to prevent double binding
        this.unbindEvents();
        
        // Interview Preparation Content events
        $('#editInterviewPrepBtn').on('click.interviewQuestions', () => this.editInterviewPrepContent());
        $('#cancelInterviewPrepBtn').on('click.interviewQuestions', () => this.cancelInterviewPrepEdit());
        $('#extractKeyPointsBtn').on('click.interviewQuestions', () => this.extractKeyPointsFromJobDescription());
        $('#saveInterviewPrepBtn').on('click.interviewQuestions', () => this.saveInterviewPrepContent());

        // Collapse/expand functionality
        $('#interviewPrepContent').on('show.bs.collapse.interviewQuestions', () => {
            $('#interviewPrepChevron').removeClass('rotated');
        });
        $('#interviewPrepContent').on('hide.bs.collapse.interviewQuestions', () => {
            $('#interviewPrepChevron').addClass('rotated');
        });

        // Questions management events
        $('#questionsCount').on('input.interviewQuestions change.interviewQuestions', () => this.validateQuestionsCountInput());
        $('#generateQuestions').on('click.interviewQuestions', () => this.generateInterviewQuestions());
        
        $('#questionsContainer').on('click.interviewQuestions', '.remove-question', (e) => this.removeQuestion(e));
        $('#questionsContainer').on('click.interviewQuestions', '.toggle-answer', (e) => this.toggleAnswer(e));
    }

    /**
     * Unbind all event handlers to prevent double binding
     */
    unbindEvents() {
        $('#editInterviewPrepBtn').off('.interviewQuestions');
        $('#cancelInterviewPrepBtn').off('.interviewQuestions');
        $('#extractKeyPointsBtn').off('.interviewQuestions');
        $('#saveInterviewPrepBtn').off('.interviewQuestions');
        $('#interviewPrepContent').off('.interviewQuestions');
        $('#questionsCount').off('.interviewQuestions');
        $('#generateQuestions').off('.interviewQuestions');
        $('#questionsContainer').off('.interviewQuestions');
    }

    /**
     * Initialize Bootstrap tooltips
     */
    initializeTooltips() {
        if (typeof bootstrap !== 'undefined') {
            const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });
        }
    }

    /**
     * Initialize interview prep word counter when entering edit mode
     */
    initializeInterviewPrepWordCounter() {
        this.interviewPrepWordCounter = new WordCounter({
            textAreaSelector: '#interviewPrepTextArea',
            wordCountSelector: '#interviewPrepWordCount',
            maxWords: 5000,
            warningThreshold: 4000,
            submitButtonSelector: '#saveInterviewPrepBtn',
            errorMessageSelector: '#interviewPrepWordLimitError'
        });
    }

    /**
     * Edit interview prep button click handler
     */
    editInterviewPrepContent() {
        $('#interviewPrepDisplay').hide();
        $('#interviewPrepEdit').show();
        $('#interviewPrepEditActions').hide();
        $('#interviewPrepSaveActions').show();
        
        // Initialize word counter
        this.initializeInterviewPrepWordCounter();
        
        // Focus on textarea
        $('#interviewPrepTextArea').focus();
    }

    /**
     * Cancel interview prep edit button click handler
     */
    cancelInterviewPrepEdit() {
        // Reset textarea to original value
        $('#interviewPrepTextArea').val(this.originalInterviewPrepContent);
        
        // Switch back to display mode
        $('#interviewPrepDisplay').show();
        $('#interviewPrepEdit').hide();
        $('#interviewPrepEditActions').show();
        $('#interviewPrepSaveActions').hide();
    }

    /**
     * Save interview prep button click handler
     */
    saveInterviewPrepContent() {
        const $button = $('#saveInterviewPrepBtn');
        const newInterviewPrepContent = $('#interviewPrepTextArea').val().trim();
        
        // Validate word count
        if (this.interviewPrepWordCounter && !this.interviewPrepWordCounter.validateWordCount().isValid) {
            return;
        }
        
        // Show loading state
        $button.prop('disabled', true);
        $button.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Saving...');
        
        // Call the save API
        this.saveInterviewPreparationContent(newInterviewPrepContent, $button);
    }

    /**
     * Save interview preparation content to the server
     */
    async saveInterviewPreparationContent(content, $button) {
        try {
            const response = await fetch(`/InterviewQuestions/api/save-preparation-content/${this.jobApplicationId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                },
                body: JSON.stringify({
                    preparationContent: content
                })
            });
            
            if (response.ok) {
                const result = await response.json();
                
                if (result.success) {
                    // Update the display with new content and original variable
                    this.originalInterviewPrepContent = content;
                    
                    if (!content) {
                        $('#interviewPrepDisplay').html(`
                            <div class="text-muted text-center py-4">
                                <i class="fas fa-edit fa-2x mb-2"></i>
                                <p class="mb-0">No interview preparation content added yet. Click "Edit" to add content or use "Extract Key Points from Job Description" to generate focused content for better interview questions.</p>
                            </div>
                        `);
                    } else {
                        $('#interviewPrepDisplay').html(content.replace(/\n/g, '<br>'));
                    }
                    
                    // Switch back to display mode
                    $('#interviewPrepDisplay').show();
                    $('#interviewPrepEdit').hide();
                    $('#interviewPrepEditActions').show();
                    $('#interviewPrepSaveActions').hide();
                    
                    // Show success message
                    this.showSuccessMessage('Interview preparation content saved successfully!', 'interviewQuestionsMessages');
                } else {
                    this.showTabMessage(
                        'Failed to save interview preparation content. Please try again.',
                        'danger',
                        'interviewQuestionsMessages'
                    );
                }
            } else {
                // Parse error response
                const errorData = await response.json().catch(() => ({ detail: 'Failed to save interview preparation content.' }));
                this.showTabMessage(
                    errorData.detail || 'Failed to save interview preparation content.',
                    'danger',
                    'interviewQuestionsMessages'
                );
            }
        } catch (error) {
            console.error('Error saving interview preparation content:', error);
            this.showTabMessage(
                'An error occurred while saving content. Please check your connection and try again.',
                'danger',
                'interviewQuestionsMessages'
            );
        } finally {
            // Reset button state
            $button.prop('disabled', false);
            $button.html('<i class="fas fa-save me-2"></i>Save');
        }
    }

    /**
     * Extract key points from job description
     */
    async extractKeyPointsFromJobDescription() {
        const $button = $('#extractKeyPointsBtn');

        const originalJobDescription = document.querySelector('pre.job-description-display');
        if (!originalJobDescription || originalJobDescription.textContent.trim() === '') {
            this.showTabMessage(
                'No job description found. Please add a job description in the Job Details tab first.',
                'warning',
                'interviewQuestionsMessages'
            );
            return;
        }
        
        // Show loading state
        $button.prop('disabled', true);
        $button.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Extracting Key Points...');
        
        // Clear previous messages
        this.clearTabMessages();
        
        try {
            const response = await fetch(`/InterviewQuestions/api/prepare-content/${this.jobApplicationId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                }
            });
            
            if (response.ok) {
                const result = await response.json();
                
                if (result.success && result.preparationContent) {
                    // Set the extracted content to the textarea
                    $('#interviewPrepTextArea').val(result.preparationContent);
                    
                    // Switch to edit mode to show the generated content
                    $('#interviewPrepDisplay').hide();
                    $('#interviewPrepEdit').show();
                    $('#interviewPrepEditActions').hide();
                    $('#interviewPrepSaveActions').show();
                    
                    // Initialize word counter if not already initialized
                    if (!this.interviewPrepWordCounter) {
                        this.initializeInterviewPrepWordCounter();
                    }
                    
                    // Focus on textarea
                    $('#interviewPrepTextArea').focus();
                    
                    // Show success message
                    this.showTabMessage(
                        'AI has successfully extracted key points from your job description. Review and edit the content as needed.',
                        'success',
                        'interviewQuestionsMessages'
                    );
                } else {
                    // Handle case where API returned success but no content
                    this.showTabMessage(
                        result.feedbackMessage || 'Unable to extract key points from the job description. Please try again.',
                        'warning',
                        'interviewQuestionsMessages'
                    );
                }
            } else {
                // Parse error response
                const errorData = await response.json().catch(() => ({ detail: 'Failed to extract key points from job description.' }));
                this.showTabMessage(
                    errorData.detail || 'Failed to extract key points from job description.',
                    'danger',
                    'interviewQuestionsMessages'
                );
            }
        } catch (error) {
            console.error('Error extracting key points:', error);
            this.showTabMessage(
                'An error occurred while extracting key points. Please check your connection and try again.',
                'danger',
                'interviewQuestionsMessages'
            );
        } finally {
            // Reset button state
            $button.prop('disabled', false);
            $button.html('<i class="fas fa-wand-magic-sparkles me-2"></i>Extract Key Points from Job Description');
        }
    }

    /**
     * Validate questions count input
     */
    validateQuestionsCountInput() {
        const $input = $('#questionsCount');
        const count = this.interviewQuestions.length;
        const remainingSlots = this.MAX_QUESTIONS - count;
        const maxAllowed = Math.min(this.MAX_QUESTIONS_PER_BATCH, remainingSlots);
        const inputValue = parseInt($input.val()) || 1;
        
        // Clear any existing validation classes
        $input.removeClass('is-invalid is-valid');
        
        if (inputValue < 1) {
            $input.val(1);
        } else if (inputValue > maxAllowed) {
            $input.addClass('is-invalid');
            $input.val(maxAllowed);
            
            // Show validation message
            if (remainingSlots < this.MAX_QUESTIONS_PER_BATCH) {
                this.showTabMessage(
                    `You can only generate ${remainingSlots} more question${remainingSlots === 1 ? '' : 's'} to reach the maximum limit of ${this.MAX_QUESTIONS}.`,
                    'warning',
                    'interviewQuestionsGenerationMessages',
                    true
                );
            } else {
                this.showTabMessage(
                    `Maximum ${this.MAX_QUESTIONS_PER_BATCH} questions can be generated per batch.`,
                    'warning', 
                    'interviewQuestionsGenerationMessages',
                    true
                );
            }
        } else {
            $input.addClass('is-valid');
        }
    }

    /**
     * Update questions counter display and check limits
     */
    updateQuestionsCounter() {
        const count = this.interviewQuestions.length;
        const $counter = $('#questionsCounter');
        const $limitMessage = $('#questionsLimitMessage');
        const $remainingSlotsInfo = $('#remainingSlotsInfo');
        const $remainingSlotsText = $('#remainingSlotsText');
        const $generateBtn = $('#generateQuestions');
        const $questionsCountInput = $('#questionsCount');
        
        // Update counter display
        $counter.text(`${count}/${this.MAX_QUESTIONS}`);
        
        // Calculate remaining slots
        const remainingSlots = this.MAX_QUESTIONS - count;
        
        // Update remaining slots info
        if (remainingSlots > 0 && remainingSlots <= 10) {
            $remainingSlotsText.text(`${remainingSlots} slot${remainingSlots === 1 ? '' : 's'} remaining`);
            $remainingSlotsInfo.show();
        } else {
            $remainingSlotsInfo.hide();
        }
        
        // Update input field constraints
        const maxAllowed = Math.min(this.MAX_QUESTIONS_PER_BATCH, remainingSlots);
        $questionsCountInput.attr('max', maxAllowed);
        
        // Validate current input value
        const currentInputValue = parseInt($questionsCountInput.val()) || 1;
        if (currentInputValue > maxAllowed) {
            $questionsCountInput.val(maxAllowed);
        }
        
        // Update counter color based on count
        $counter.removeClass('bg-secondary bg-warning bg-danger');
        if (count >= this.MAX_QUESTIONS) {
            $counter.addClass('bg-danger');
            $limitMessage.show();
        } else if (count >= this.MAX_QUESTIONS * 0.8) { // 80% of limit (16 questions)
            $counter.addClass('bg-warning');
            $limitMessage.hide();
        } else {
            $counter.addClass('bg-secondary');
            $limitMessage.hide();
        }
        
        // Disable/enable generate buttons based on limit
        const hasReachedLimit = count >= this.MAX_QUESTIONS;
        $generateBtn.prop('disabled', hasReachedLimit);
        $questionsCountInput.prop('disabled', hasReachedLimit);
        
        // Update button text based on question count and limit status
        if (hasReachedLimit) {
            $generateBtn.html('Generate (Limit Reached)');
        } else if (count > 0) {
            $generateBtn.html('Generate More');
        } else {
            $generateBtn.html('Generate');
        }
    }

    /**
     * Check if user can generate more questions
     */
    canGenerateMoreQuestions() {
        return this.interviewQuestions.length < this.MAX_QUESTIONS;
    }

    /**
     * Generate more questions button click handler
     */
    generateMoreQuestions() {
        // Set a reasonable default for "Generate More" if the current value is too high
        const currentValue = parseInt($('#questionsCount').val()) || 5;
        const remainingSlots = this.MAX_QUESTIONS - this.interviewQuestions.length;
        const maxAllowed = Math.min(this.MAX_QUESTIONS_PER_BATCH, remainingSlots);
        
        if (currentValue > maxAllowed) {
            $('#questionsCount').val(Math.min(5, maxAllowed));
        }
        
        this.generateInterviewQuestions();
    }

    /**
     * Fetch existing interview questions from the server
     */
    async fetchExistingInterviewQuestions(showLoading = true) {
        this.questionsLoaded = true;
        
        if (showLoading) {
            $('#questionsContainer').html(`
                <div class="text-center py-5">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <p class="mt-3 text-muted">Loading interview questions...</p>
                </div>
            `);
        }
        
        try {
            const response = await fetch(`/InterviewQuestions/api/fetch/${this.jobApplicationId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.detail || 'Failed to fetch interview questions.');
            }
            
            const data = await response.json();
            
            // Set the original interview preparation content if it exists
            if (data.preparationContent) {
                this.originalInterviewPrepContent = data.preparationContent;
                $('#interviewPrepDisplay').html(data.preparationContent.replace(/\n/g, '<br>'));
                $('#interviewPrepTextArea').val(data.preparationContent);
            }
            
            // Show section-level feedback if present
            if (data.feedbackMessage && data.status) {
                const shouldAutoHide = data.status && data.status.toLowerCase() === 'success';
                this.showTabMessage(data.feedbackMessage, data.status, 'interviewQuestionsGenerationMessages', shouldAutoHide);
            }
            
            const questionsArray = data.interviewQuestions || [];
            
            if (questionsArray && questionsArray.length > 0) {
                this.interviewQuestions = questionsArray;
                this.renderInterviewQuestions();
                this.updateQuestionsCounter(); // Update counter after loading questions
            } else {
                this.interviewQuestions = [];
                $('#questionsContainer').html(`
                    <div class="text-muted text-center py-5">
                        <i class="fas fa-question-circle fa-3x mb-3"></i>
                        <p>Click "Generate" to create interview questions based on provided preparation content.</p>
                    </div>
                `);
                this.updateQuestionsCounter(); // Update counter even when no questions
            }
            
        } catch (error) {
            console.error('Error fetching interview questions:', error);
            this.showTabMessage(`Failed to load interview questions: ${error.message}`, 'error', 'interviewQuestionsGenerationMessages');
            $('#questionsContainer').html(`
                <div class="text-muted text-center py-5">
                    <i class="fas fa-question-circle fa-3x mb-3"></i>
                    <p>Click "Generate" to create interview questions based on provided preparation content.</p>
                </div>
            `);
            this.interviewQuestions = []; // Reset questions array on error
            this.updateQuestionsCounter(); // Update counter on error
        }
    }

    /**
     * Generate interview questions
     */
    async generateInterviewQuestions() {
        const $generateBtn = $('#generateQuestions');
        const $generateMoreBtn = $('#generateMoreQuestions');
        const activeBtn = $generateBtn.is(':visible') ? $generateBtn : $generateMoreBtn;
        
        // Get and validate the requested number of questions
        const requestedCount = parseInt($('#questionsCount').val()) || 5;
        const currentCount = this.interviewQuestions.length;
        const remainingSlots = this.MAX_QUESTIONS - currentCount;
        const maxAllowed = Math.min(this.MAX_QUESTIONS_PER_BATCH, remainingSlots);
        
        // Validate the requested count
        if (requestedCount > maxAllowed) {
            if (remainingSlots === 0) {
                this.showTabMessage(
                    `You've reached the maximum limit of ${this.MAX_QUESTIONS} interview questions. Please remove some questions if you want to generate new ones.`,
                    'warning',
                    'interviewQuestionsGenerationMessages'
                );
            } else if (remainingSlots < this.MAX_QUESTIONS_PER_BATCH) {
                this.showTabMessage(
                    `You can only generate ${remainingSlots} more question${remainingSlots === 1 ? '' : 's'} to reach the maximum limit of ${this.MAX_QUESTIONS}. Please adjust the number and try again.`,
                    'warning',
                    'interviewQuestionsGenerationMessages'
                );
                $('#questionsCount').val(remainingSlots);
            } else {
                this.showTabMessage(
                    `Maximum ${this.MAX_QUESTIONS_PER_BATCH} questions can be generated per batch. Please adjust the number and try again.`,
                    'warning',
                    'interviewQuestionsGenerationMessages'
                );
                $('#questionsCount').val(this.MAX_QUESTIONS_PER_BATCH);
            }
            return;
        }
        
        // Check if we've reached the maximum questions limit
        if (!this.canGenerateMoreQuestions()) {
            this.showTabMessage(
                `You've reached the maximum limit of ${this.MAX_QUESTIONS} interview questions. Please remove some questions if you want to generate new ones.`,
                'warning',
                'interviewQuestionsGenerationMessages'
            );
            return;
        }
        
        // Check if there's interview preparation content
        const interviewPrepContent = this.originalInterviewPrepContent || $('#interviewPrepTextArea').val();
        
        if (!interviewPrepContent || interviewPrepContent.trim() === '') {
            this.showTabMessage(
                'No interview preparation content found. Please add content above or use "Extract Key Points from Job Description" to generate focused content for better interview questions.',
                'warning',
                'interviewQuestionsGenerationMessages'
            );
            return;
        }
        
        // Show loading state
        activeBtn.prop('disabled', true);
        activeBtn.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Generating...');
        
        // Clear previous messages
        this.clearTabMessages();
        
        try {
            const response = await fetch(`/InterviewQuestions/api/generate/${this.jobApplicationId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                },
                body: JSON.stringify({
                    questionsCount: requestedCount
                })
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.detail || 'Failed to generate interview questions.');
            }
            
            const data = await response.json();
            
            // Handle InterviewQuestionsSectionViewModel response structure
            // Handle both camelCase and PascalCase property names (depends on serialization settings)
            const status = data.status || data.Status;
            const feedbackMessage = data.feedbackMessage || data.FeedbackMessage;
            const newInterviewQuestions = data.interviewQuestions || data.InterviewQuestions || [];
            
            if (status === 'Error') {
                this.showTabMessage(feedbackMessage || 'Unable to generate interview questions.', 'error', 'interviewQuestionsGenerationMessages');
                return;
            }
            
            const questionsCount = newInterviewQuestions.length;
            
            // Show message about requested vs generated
            let successMessage = feedbackMessage;
            if (!successMessage) {
                if (questionsCount === requestedCount) {
                    successMessage = `Successfully generated ${questionsCount} interview question${questionsCount === 1 ? '' : 's'}.`;
                } else {
                    successMessage = `Successfully generated ${questionsCount} interview question${questionsCount === 1 ? '' : 's'} (requested ${requestedCount}).`;
                }
            }
            
            this.showTabMessage(successMessage, 'success', 'interviewQuestionsGenerationMessages', true);
            
            // Process and display the questions from the API response
            // The API returns the complete list of all interview questions for this job application
            if (newInterviewQuestions && newInterviewQuestions.length > 0) {
                // Simply replace the entire questions array with the API response
                // The API handles persistence and returns the complete, up-to-date list
                this.interviewQuestions = newInterviewQuestions;
                
                this.renderInterviewQuestions();
                this.updateQuestionsCounter(); // Update counter after setting questions
            } else {
                // No questions returned - clear the array and show empty state
                this.interviewQuestions = [];
                $('#questionsContainer').html(`
                    <div class="text-muted text-center py-5">
                        <i class="fas fa-question-circle fa-3x mb-3"></i>
                        <p>No questions were generated. Please improve your interview preparation content and try again.</p>
                    </div>
                `);
                this.updateQuestionsCounter(); // Still update counter
            }
            
        } catch (error) {
            console.error('Error generating interview questions:', error);
            this.showTabMessage(error.message, 'error', 'interviewQuestionsGenerationMessages');
        } finally {
            // Reset button state, but respect the counter limits
            activeBtn.prop('disabled', false);
            if (activeBtn.attr('id') === 'generateQuestions') {
                activeBtn.html('Generate');
            } else {
                activeBtn.html('Generate More Questions');
            }
            
            // Update counter to ensure buttons are in correct state
            this.updateQuestionsCounter();
        }
    }

    /**
     * Render interview questions in the DOM
     */
    renderInterviewQuestions() {
        $('#questionsContainer').empty();
        
        if (this.interviewQuestions.length === 0) {
            $('#questionsContainer').html(`
                <div class="text-muted text-center py-5">
                    <i class="fas fa-question-circle fa-3x mb-3"></i>
                    <p>Click "Generate" to create interview questions based on provided preparation content.</p>
                </div>
            `);
            return;
        }
        
        let questionsHtml = '';
        
        this.interviewQuestions.forEach((question, index) => {
            // Handle both camelCase and PascalCase property names
            const questionText = question.question || question.Question || 'No question available';
            const answerText = question.answer || question.Answer || 'No answer available';
            const guideText = question.guide || question.Guide || 'No guide available';
            
            questionsHtml += `
                <div class="question-item mb-3" data-index="${index}">
                    <div class="card">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <div class="flex-grow-1 d-flex align-items-center">
                                <h6 class="mb-0 me-2">Question ${index + 1}</h6>
                            </div>
                            <div class="d-flex gap-2">
                                <button type="button" class="btn btn-outline-primary btn-sm toggle-answer" data-index="${index}">
                                    <i class="fas fa-eye me-1"></i>Show Answer
                                </button>
                                <div class="dropdown">
                                    <button type="button" class="btn btn-outline-secondary btn-sm" data-bs-toggle="dropdown" aria-expanded="false">
                                        <i class="fas fa-ellipsis-v"></i>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-end">
                                        <li><button type="button" class="dropdown-item text-danger remove-question" data-index="${index}">
                                            <i class="fas fa-trash me-2"></i>Delete Question
                                        </button></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="question-section">
                                <h6 class="text-primary mb-2"><i class="fas fa-question-circle me-2"></i>Question</h6>
                                <p class="question-text mb-0">${questionText}</p>
                            </div>
                            
                            <div class="answer-section mt-3" style="display: none;">
                                <hr>
                                <div class="row">
                                    <div class="col-md-6">
                                        <h6 class="text-info mb-2"><i class="fas fa-compass me-2"></i>Answer Guide</h6>
                                        <p class="guide-text">${guideText}</p>
                                    </div>
                                    <div class="col-md-6">
                                        <h6 class="text-success mb-2"><i class="fas fa-lightbulb me-2"></i>Suggested Answer</h6>
                                        <p class="answer-text">${answerText}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        });
        
        // Remove the 'Generate More' button and limit message at the bottom
        $('#questionsContainer').html(questionsHtml);
    }

    /**
     * Toggle answer visibility for a question
     */
    toggleAnswer(e) {
        const questionIndex = $(e.target).data('index');
        const $questionItem = $(`.question-item[data-index="${questionIndex}"]`);
        const $answerSection = $questionItem.find('.answer-section');
        const $toggleBtn = $questionItem.find('.toggle-answer');
        
        if ($answerSection.is(':visible')) {
            $answerSection.slideUp();
            $toggleBtn.html('<i class="fas fa-eye me-1"></i>Show Answer');
        } else {
            $answerSection.slideDown();
            $toggleBtn.html('<i class="fas fa-eye-slash me-1"></i>Hide Answer');
        }
    }

    /**
     * Remove a question with confirmation
     */
    removeQuestion(e) {
        e.preventDefault(); // Prevent any default behavior
        e.stopPropagation(); // Stop event bubbling
        
        // Prevent multiple simultaneous deletions
        if (this.isDeletingQuestion) {
            console.log('Already deleting a question, ignoring duplicate call');
            return;
        }
        
        const questionIndex = $(e.currentTarget).data('index');
        
        // Validate question index
        if (questionIndex === undefined || questionIndex < 0 || questionIndex >= this.interviewQuestions.length) {
            console.error('Invalid question index:', questionIndex);
            return;
        }
        
        if (confirm('Are you sure you want to remove this question?')) {
            // Set flag to prevent multiple deletions
            this.isDeletingQuestion = true;
            
            const question = this.interviewQuestions[questionIndex];
            
            // Handle both camelCase and PascalCase property names from API response
            const questionId = question && (question.id || question.Id);
            
            // If the question has an ID, it means it's saved in the database and we need to delete it via API
            if (questionId) {
                this.deleteInterviewQuestion(questionId, questionIndex);
            } else {
                // If no ID, it's a newly generated question that hasn't been saved yet, just remove from array
                this.interviewQuestions.splice(questionIndex, 1);
                this.renderInterviewQuestions();
                this.updateQuestionsCounter(); // Update counter after removing question
                
                // Show success message
                this.showTabMessage('Question removed successfully.', 'success', 'interviewQuestionsGenerationMessages', true);
                
                // Reset flag
                this.isDeletingQuestion = false;
            }
        }
    }

    /**
     * Delete interview question via API
     */
    async deleteInterviewQuestion(questionId, questionIndex) {
        try {
            const response = await fetch(`/InterviewQuestions/api/delete/${questionId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                }
            });

            if (response.ok) {
                const result = await response.json();
                this.showTabMessage(result.message, 'success', 'interviewQuestionsGenerationMessages', true);
                await this.fetchExistingInterviewQuestions(false);
                this.updateQuestionsCounter(); // Update counter after deleting question
            } else {
                const errorData = await response.json();
                this.showTabMessage(errorData.detail || 'An error occurred while deleting the question.', 'error', 'interviewQuestionsGenerationMessages');
            }
        } catch (error) {
            console.error('Error deleting interview question:', error);
            this.showTabMessage('An error occurred while deleting the question. Please try again.', 'error', 'interviewQuestionsGenerationMessages');
        } finally {
            // Always reset the deletion flag, regardless of success or failure
            this.isDeletingQuestion = false;
        }
    }

    /**
     * Clear all tab messages
     */
    clearTabMessages() {
        // Clear all messages from all tabs
        $('#jobDetailsMessages .alert').remove();
        $('#interviewQuestionsMessages .alert').remove();
        $('#interviewQuestionsGenerationMessages .alert').remove();
    }

    /**
     * Show a tab-specific message
     */
    showTabMessage(message, type = 'info', containerId = 'jobDetailsMessages', autoHide = false) {
        // Clear existing messages in this container first
        $(`#${containerId} .alert`).remove();
        
        let alertClass = 'alert-info';
        let iconClass = 'fas fa-info-circle';
        
        switch (type.toLowerCase()) {
            case 'success':
                alertClass = 'alert-success';
                iconClass = 'fas fa-check-circle';
                break;
            case 'warning':
                alertClass = 'alert-warning';
                iconClass = 'fas fa-exclamation-triangle';
                break;
            case 'error':
            case 'danger':
                alertClass = 'alert-danger';
                iconClass = 'fas fa-exclamation-circle';
                break;
        }
        
        const messageHtml = `
            <div class="alert ${alertClass} alert-dismissible fade show mt-2" role="alert">
                <i class="${iconClass} me-2"></i>${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        
        const $newAlert = $(messageHtml);
        $(`#${containerId}`).prepend($newAlert);
        
        if (autoHide) {
            const hideDelay = type.toLowerCase() === 'success' ? 8000 : 8000;
            setTimeout(() => {
                $newAlert.fadeOut(300, function() {
                    $(this).remove();
                });
            }, hideDelay);
        }
    }

    /**
     * Cleanup method to properly unbind events and reset state
     */
    cleanup() {
        this.unbindEvents();
        this.isDeletingQuestion = false;
        if (this.interviewPrepWordCounter) {
            // Clean up word counter if it has a cleanup method
            if (typeof this.interviewPrepWordCounter.cleanup === 'function') {
                this.interviewPrepWordCounter.cleanup();
            }
            this.interviewPrepWordCounter = null;
        }
    }
}

// Export for use in main view
window.InterviewQuestionsManager = InterviewQuestionsManager;
