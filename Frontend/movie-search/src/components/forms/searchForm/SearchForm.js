import React, { useState } from "react";
import { SearchWrapper } from './SearchForm.styles';
import useDebounce from '../../../services/hooks/useDebounce';
import _debounce from 'lodash/debounce';
import { Field, reduxForm, submit } from 'redux-form';
import { compose } from 'redux';

const renderField = ({ input: { onChange, value }, placeholder }) => {
  console.log(value)
  const handleChange=(e)=>{
    onChange(e.target.value)
  }
debugger;
  return (
      <input
        placeholder={ placeholder }
        onChange={ handleChange }
        value={ value && value.target.value }
      />
  );
};


const Search = (props) => {
  const [searchValue, setSearchValue] = useState("");
  // const debouncedSearchTerm = useDebounce(searchValue, 500);
console.log(props)
 /* const handleSearchInputChanges = (e) =>{
    /!*setSearchValue(e.target.value);
    if(debouncedSearchTerm) {
      props.search(searchValue)
    }*!/

    _debounce(props.search(e.target.value), 300)

  }*/

//const debouncedSearch=(query)=> _debounce(()=>search(query), 100)

   const handleSearchInputChanges = (e) =>{

       //_debounce(()=>search(e.target.value), 100)
     //debouncedSearch(e.target.value)

     //search(e.target.value)

   }


  /*const handleSearchInputChanges = (e) = {
    _debounce(search(e.target.value), 300)
  }*/

  return (
    <SearchWrapper>
      <form className="search">
        <Field
          name="query"
          component={ renderField }
          placeholder="Enter Password"
        />
        {/*<input
          //value={searchValue}
          onChange={ handleSearchInputChanges }
          type="text"
        />*/}
      </form>
    </SearchWrapper>
  );
}

export default compose(
  reduxForm({
    onChange: _debounce((values, dispatch, props) => {
      props.submit()
    }, 300),
    enableReinitialize: true,
  }),
)(Search);

