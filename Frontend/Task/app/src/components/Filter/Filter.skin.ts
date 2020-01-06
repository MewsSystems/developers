export const FilterElement = `
    display: flex;
    padding-top: 1rem;
`;
export const FilterCheckbox = `
    display: none;
    &:checked+span {
        background: #fff;
        color: #000;
        transition: background .5s ease; 
    }
`;
export const FilterPairName = `
    min-width: 3rem;
    border-radius: .3rem;
    border: 1px solid #fff;
    padding: .5rem;
    margin: 0 .5rem;
    transition: background .5s ease; 
`;

