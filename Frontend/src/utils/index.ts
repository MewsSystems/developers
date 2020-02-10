export const formatMoney = (amount: string | number) => {
  if (!amount) {
    return '';
  }

  let copy = amount.toString().split('').reverse();
  const output = [];
  while (copy.length > 0) {
    output.push(copy.splice(0, 3).join(''));
  }

  return `$${output.reverse().join(' ')}`;
};

export const formatDuration = (duration: number) => {
  if (!duration) {
    return '';
  }

  const hours = Math.floor(duration / 60);
  const minutes = duration % 60;
  let output = '';
  
  if (hours > 0){
    const hourPlural = hours > 1 ? 's' : '';
    output +=`${hours} Hour${hourPlural}`;
  }
  if (minutes > 0) {
    output +=` ${minutes} Minutes`;
  }

  return output;
}
