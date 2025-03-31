module LoggingHelper
  # Log a message with specified severity
  # @param message [String] Message to log
  # @param level [Symbol] Log level (:error, :warn, :info)
  # @param prefix [String] Optional prefix for the message
  def log_message(message, level = :error, prefix = nil)
    full_message = prefix ? "#{prefix}: #{message}" : message
    
    if defined?(Rails)
      case level
      when :warn, :warning
        Rails.logger.warn(full_message)
      when :info
        Rails.logger.info(full_message)
      else
        Rails.logger.error(full_message)
      end
    else
      if level == :info
        puts(full_message)
      else
        warn("[#{level.to_s.upcase}] #{full_message}")
      end
    end
  end
  
  # Log an error message (for backward compatibility)
  # @param message [String] Error message
  def log_error(message)
    log_message(message, :error)
  end
  
  # Log a warning message (for backward compatibility)
  # @param message [String] Warning message
  def log_warning(message)
    log_message(message, :warn)
  end
  
  # Log an info message (for backward compatibility)
  # @param message [String] Info message
  def log_info(message)
    log_message(message, :info)
  end
end 