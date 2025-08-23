import GridSection from "../../components/GridSection";

export default function HomeRails() {
    return (
        <div className="space-y-18">
            <GridSection
                title="Most Recent"
                endpoint="/movie/now_playing"
                limit={12}
            />
            <GridSection
                title="Upcoming"
                endpoint="/movie/upcoming"
                limit={12}
            />
            <GridSection title="Popular" endpoint="/movie/popular" limit={12} />
        </div>
    );
}
