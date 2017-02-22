/// <reference path="../Scripts/typings/moment/moment.d.ts" />
/// <reference path="../Scripts/typings/moment-timezone/moment-timezone.d.ts" />

class LocalDateCreator {

    static getLocalDate(date: string): string {
        var tz = moment.tz.guess();
        var formattedDate = moment.tz(date, tz).format('MM.DD.YYYY hh:mm a').toUpperCase();
        return formattedDate;
    }

}