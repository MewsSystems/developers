import styled from 'styled-components';

export const TableShellContainer = styled.div`
    &.Table {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        flex: 1 1 0;
        width: 100%;
        font-size: 13px;
        margin-top: 20px;
        margin-bottom: 20px;
    }

    .Table__Header {
        flex: 1 1 0;
        width: 100%;
    }

    .Table__Body {
        flex: 1 1 0;
        width: 100%;
    }

    .Table__Row--header {
        background-color: #f5f5f5;
        font-weight: bold;
    }

    .Table__Row {
        display: flex;
        flex: 1 1 0;
        width: 100%;
        border-bottom: 1px solid #ddd;
        cursor: pointer;
    }

    .Table__Cell {
        flex: 1 1 auto;
        width: 100%;
        display: flex;
        align-items: center;
        padding: 10px;

        .Item {
            width: 100%;
        }
    }

    .Table__Cell--header {
        justify-content: flex-end;
    }

    .Table__Cell--body {
        padding: 10px 0 10px 10px;
        width: 20px;
    }

    .TableShell__Filters {
        background-color: #f5f5f5;
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 10px 0 10px 10px;
    }
`;


