import React from "react";
import {HelloWorld} from "./HelloWorld";
import {screen, render} from "@testing-library/react";

describe("When the component renders", () => {
    it('should show a "Hello World" message', () => {
        render(<HelloWorld />)
        const text = screen.getByText('Hemllo World')

        expect(text).toBeInTheDocument()
    })
})
