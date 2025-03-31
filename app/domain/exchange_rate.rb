class ExchangeRate
  attr_reader :from, :to, :rate, :date

  def initialize(from:, to:, rate:, date: Date.today)
    @from = from.is_a?(Currency) ? from : Currency.new(from)
    @to = to.is_a?(Currency) ? to : Currency.new(to)
    @rate = Float(rate)
    @date = date
    raise ArgumentError, 'Rate must be positive' if @rate <= 0
  end

  def inverse
    raise 'Inverse rate calculation is not allowed'
  end
  
  def to_h
    {
      from: from.code,
      to: to.code,
      rate: rate
    }
  end
end 