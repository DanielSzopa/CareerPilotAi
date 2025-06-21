/**
 * Word Counter Utility
 * Provides reusable word counting functionality for textareas
 */
class WordCounter {
    constructor(options) {
        this.textAreaSelector = options.textAreaSelector;
        this.wordCountSelector = options.wordCountSelector;
        this.maxWords = options.maxWords || 5000;
        this.warningThreshold = options.warningThreshold || 4000;
        this.submitButtonSelector = options.submitButtonSelector;
        this.errorMessageSelector = options.errorMessageSelector;
        
        this.init();
    }
    
    init() {
        const textArea = $(this.textAreaSelector);
        if (textArea.length === 0) {
            console.warn('WordCounter: Text area not found with selector:', this.textAreaSelector);
            return;
        }
        
        // Bind input event
        textArea.on('input', () => this.validateWordCount());
        
        // Initial validation
        this.validateWordCount();
    }
    
    countWords(text) {
        if (!text) return 0;
        text = text.trim();
        return text === '' ? 0 : text.split(/\s+/).length;
    }
    
    validateWordCount() {
        const text = $(this.textAreaSelector).val();
        const wordCount = this.countWords(text);
        const wordCountElement = $(this.wordCountSelector);
        
        // Update word count display
        wordCountElement.text(wordCount.toLocaleString());
        
        // Remove existing classes
        wordCountElement.removeClass('text-danger text-warning');
        wordCountElement.parent().removeClass('text-danger');
        
        // Handle error message visibility
        if (this.errorMessageSelector) {
            $(this.errorMessageSelector).hide();
        }
        
        // Handle submit button state
        if (this.submitButtonSelector) {
            $(this.submitButtonSelector).prop('disabled', false);
        }
        
        // Apply validation logic
        if (wordCount > this.maxWords) {
            // Exceeded maximum
            wordCountElement.addClass('text-danger');
            wordCountElement.parent().addClass('text-danger');
            
            if (this.errorMessageSelector) {
                $(this.errorMessageSelector).show();
            }
            
            if (this.submitButtonSelector) {
                $(this.submitButtonSelector).prop('disabled', true);
            }
        } else if (wordCount > this.warningThreshold) {
            // Warning threshold
            wordCountElement.addClass('text-warning');
        }
        
        return {
            wordCount: wordCount,
            isValid: wordCount <= this.maxWords,
            isWarning: wordCount > this.warningThreshold && wordCount <= this.maxWords
        };
    }
    
    // Static method for simple word counting without validation
    static countWords(text) {
        if (!text) return 0;
        text = text.trim();
        return text === '' ? 0 : text.split(/\s+/).length;
    }
} 