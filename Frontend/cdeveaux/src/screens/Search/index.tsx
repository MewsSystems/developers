import React from 'react';
import { Link } from 'react-router-dom';

class Search extends React.PureComponent<{}> {
  render() {
    const items = [{
      id: '1223056169810350',
      title: 'random title 1',
    }, {
      id: '8693203230429313',
      title: 'random titel 2',
    }];

    return (
      <>
        <input type="text"/>
        <ul>
          {items.map(item => (
            <Link key={item.id} to={item.id}>
              {item.title}
            </Link>
          ))}
        </ul>
      </>
    );
  }
};

export default Search;
