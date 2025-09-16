import { getConfiguration } from "@/entities/configuration/api/configurationApi";
import { getMovie } from "@/entities/movie/api/getMovieApi";
import { getMovieCollection } from "@/entities/movie/api/getMovieCollectionApi";
import { getMovieImages } from "@/entities/movie/api/getMovieImagesApi";
import { includeConfiguration, toProductionComponiesWithImg } from "@/shared/lib/utils";

export async function getDetails({ movie_id }: { movie_id: string }, { language, session_id }: { language: string, session_id: string }) {
    const movie = await getMovie({ movie_id }, { language, session_id });
    const images = await getMovieImages({ movie_id });
    const collection = movie?.belongs_to_collection ? await getMovieCollection({ collection_id: movie.belongs_to_collection.id }, { language }) : undefined;
    const configuration = await getConfiguration()
    if (movie == undefined) {
        throw Error("no movie found for this movie");
    }

    if (configuration == undefined) {
        throw Error("no configuration found");
    }

    const boundIncludeConfiguration = includeConfiguration.bind(null, configuration.images);
    const productionCompaniesWithImg = toProductionComponiesWithImg(boundIncludeConfiguration, movie.production_companies);
    return {
        movie,
        images,
        productionCompaniesWithImg,
        boundIncludeConfiguration,
        configuration,
        collection,
        language
    };
}
