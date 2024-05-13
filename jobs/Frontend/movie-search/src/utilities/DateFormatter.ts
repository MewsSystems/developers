import moment from 'moment';

const formatDate = (dateString: string, inputFormat: string, outputFormat: string): string => {
    const parsedDate = moment(dateString, inputFormat);
    const formattedDate = parsedDate.format(outputFormat);

    return formattedDate;
};

export default formatDate;