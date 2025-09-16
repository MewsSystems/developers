import type { Configuration } from "@/entities/configuration/types";
import type { Collection, Images, MovieDetailsAppended, MovieProductionCompanyWithImg } from "@/entities/movie/types";

export type DetailsProps = {
    movie: MovieDetailsAppended;
    collection?: Collection;
    configuration: Configuration;
    images?: Images;
    language: string;
    productionCompaniesWithImg: MovieProductionCompanyWithImg[];
    boundIncludeConfiguration: (path: string, sizePosition?: number | undefined) => string;
}
