import React from "react";
import {expect} from 'chai';
import { mount } from 'enzyme';

import {MovieDetail} from "../components/movieDetail";
import {MovieDetails} from "../types/movies";

describe('movieDetail Component Tests',  () => {
    test('renders ok with some data', () => {
        const MOVIE_TITLE = "dummyMovie"
        const MOVIE_RUNTIME = 90
        const MOVIE_STATUS = "Released"

        const someMovie: MovieDetails = {
            title: MOVIE_TITLE,
            runtime: MOVIE_RUNTIME,
            status: MOVIE_STATUS
        }

        const wrapper = mount(<MovieDetail movie={someMovie} />);
        expect(wrapper.find(`h2[id="MovieDetailTitle"]`).text()).to.equal(MOVIE_TITLE);
        expect(wrapper.find(`p[id="MovieDetailRuntime"]`).text()).to.contain(`${MOVIE_RUNTIME} Minutes` );
        expect(wrapper.find(`p[id="MovieDetailReleased"]`).find("span").text()).to.equal("✅");
    });
})