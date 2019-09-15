import { Trend } from "@store/types";

export const getTrend = (oldRate: number, newRate: any): Trend => {
    if(newRate < oldRate) {
        return "declining"
    } else if (newRate == oldRate) {
        return "stagnating"
    } else {
        return "growing"
    }
}