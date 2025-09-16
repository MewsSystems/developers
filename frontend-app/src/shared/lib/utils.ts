import type { ConfigurationImages } from '@/entities/configuration/types';
import type { MovieProductionCompanies, MovieProductionCompanyWithImg } from '@/entities/movie/types';
import { DateTime, Duration } from 'luxon';

export function toLocaleDate(date: string, language: string) {
    return DateTime.fromISO(date).setLocale(language).toLocaleString()
}

export function toLocaleYear(date: string, language: string) {
    return DateTime.fromISO(date).setLocale(language).toLocaleString({ year: 'numeric' })
}

export function toDurationFormat(durationMin: number) {
    if (durationMin > 60) {
        return Duration.fromObject({ minutes: durationMin }).normalize().toFormat('h\'h\' m\'m\'')
    }
    return `${durationMin}m`
}

export function getFlagEmoji(countryCode: string) {
    const codePoints = countryCode
        .toUpperCase()
        .split('')
        .map(char => 127397 + char.charCodeAt(0));
    return String.fromCodePoint(...codePoints);
}

export function formatDollar(price: number) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    }).format(price);
}


export enum YOUTUBESIZES {
    HQ = 'hqdefault',
    SD = 'sddefault',
    MQ = 'mqdefault'
}

export function createYoutubeImgUrl(key: string, size?: YOUTUBESIZES) {
    const keysize = size ? size : 'default';
    return `https://img.youtube.com/vi/${key}/${keysize}.jpg`;
}


export function includeConfiguration(configurationImages: ConfigurationImages, path: string, sizePosition: number = 1) {
    const sizes = configurationImages.poster_sizes;
    const position = sizePosition > sizes.length - 1 ? 0 : sizePosition;
    return configurationImages.base_url + sizes[position] + path;
}

export function parseBackdropToImgPath(configurationImages: ConfigurationImages, path: string, sizePosition: number = 1) {
    if (!path) {
        return ""
    }
    const sizes = configurationImages.backdrop_sizes;
    const position = sizePosition > sizes.length - 1 ? 0 : sizePosition;
    return configurationImages.base_url + sizes[position] + path;
}

export function parsePosterToImgPath(configurationImages: ConfigurationImages, path: string, sizePosition: number = 1) {
    const sizes = configurationImages.poster_sizes;
    const position = sizePosition > sizes.length - 1 ? 0 : sizePosition;
    return configurationImages.base_url + sizes[position] + path;
}

export function parseProfileToImgPath(configurationImages: ConfigurationImages, path: string, sizePosition: number = 1) {
    const sizes = configurationImages.profile_sizes;
    const position = sizePosition > sizes.length - 1 ? 0 : sizePosition;
    return configurationImages.base_url + sizes[position] + path;
}

export function parseWidth(widthString: string): string {
    if (widthString == "original") {
        return "100%";
    }
    return parseInt(widthString.replace("w", "")) + "px";
}

export function toProductionComponiesWithImg(boundIncludeConfiguration: (path: string) => string, productionCompanies: MovieProductionCompanies[]): MovieProductionCompanyWithImg[] {
    return productionCompanies.map(p => {
        const logo_img = p.logo_path ? boundIncludeConfiguration(p.logo_path) : undefined
        return {
            ...p,
            logo_img
        }
    });
}

export function formatList(words: string[], language: string) {
    const formatter = new Intl.ListFormat(language, {
        style: "long",
        type: "conjunction",
    });
    return formatter.format(words);
}