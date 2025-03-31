class Currency
  attr_reader :code, :name

  def initialize(code, name = nil)
    # If code is already a Currency, just use its values
    if code.is_a?(Currency)
      @code = code.code
      @name = name || code.name
      return
    end

    raise ArgumentError, "Currency code must be provided" if code.nil? || code.strip.empty?
    @code = code.strip.upcase
    @name = name
  end

  def ==(other)
    other.is_a?(Currency) && code == other.code
  end

  alias eql? ==
  def hash = code.hash
  def to_s = code
end 