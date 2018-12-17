import styled, { css } from "styled-components";


export const TableStyles = css`
    border-collapse: collapse;
    width: 100%;
    border: 2px solid #282828;
    box-shadow: 0 0 8px 0 rgba(0, 0, 0, 0.4);
`;

export const ThStyles = css`
    text-align: left;
    font-weight: normal;
    font-family: "Mina";
    font-size: 20pt;
    color: ${p => (p && p.className == "SelectorList") ? "black" : "white"};

    padding-right: 2%;

    white-space: nowrap;

    ${p => p && p.className == "RateList" && css`
        &:nth-of-type(1) {
            padding-left: 2%;
            padding-right: 3em;
        }

        &:nth-of-type(2) {
            text-align: right;
        }
    `}
`;

export const TrStyles = css`
    ${p => p && p.className == "SelectorList" && css`
        background-color: ${p => (p && p.type == "checked") ? "#ffa500" : "#333"};

        &:hover {
            background-color: ${p => (p && p.type == "checked") ? "#ffba3a" : "#555"};
        }
    `}

    ${p => p && p.className == "RateList" && css`
        &:nth-child(even) {
            background-color: #272727;
        }

        &:hover {
            background-color: #555;
        }
    `}
`;
