import React from "react";
import { Table, Menu, Icon } from "semantic-ui-react";

export interface TableFooterProps {
  currPage: number;
  totalPages: number;
  changePage: (page: number | "next" | "prev") => void;
}

const TableFooter: React.FC<TableFooterProps> = ({
  currPage,
  totalPages,
  changePage
}) => {
  const pageLinks: JSX.Element[] = [];
  for (
    let n = Math.max(1, currPage - 2);
    (n <= totalPages && n <= currPage + 2) ||
    n <= Math.max(1, currPage - 2) + 5;
    n++
  ) {
    pageLinks.push(
      <Menu.Item
        key={"pagination_" + n}
        as="a"
        onClick={() => {
          changePage(n);
        }}
      >
        {n}
      </Menu.Item>
    );
  }

  return (
    <Table.Footer>
      {totalPages > 1 && (
        <Table.Row>
          <Table.HeaderCell colSpan="3">
            <Menu floated="right" pagination>
              {currPage > 1 && (
                <Menu.Item
                  as="a"
                  icon
                  onClick={() => {
                    changePage("prev");
                  }}
                >
                  <Icon name="chevron left" />
                </Menu.Item>
              )}
              {pageLinks}
              {currPage < totalPages && (
                <Menu.Item
                  as="a"
                  icon
                  onClick={() => {
                    changePage("next");
                  }}
                >
                  <Icon name="chevron right" />
                </Menu.Item>
              )}
            </Menu>
          </Table.HeaderCell>
        </Table.Row>
      )}
    </Table.Footer>
  );
};

export default TableFooter;
