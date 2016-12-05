module.exports = function adjustDecimal(value, exp) {
    value = value.toString().split('e');
    value = Math.floor(+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
}