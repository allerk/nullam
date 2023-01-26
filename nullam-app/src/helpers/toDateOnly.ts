export default function toDateOnly(dateTime: string): string {
    const dateObject = new Date(dateTime);
    let day = dateObject.getDate().toString();
    let month = (dateObject.getMonth() + 1).toString();
    let year = dateObject.getFullYear().toString();

    if (day.length === 1) day = "0" + day;
    if (month.length === 1) month = "0" + month;

    return `${day}.${month}.${year}`;
}