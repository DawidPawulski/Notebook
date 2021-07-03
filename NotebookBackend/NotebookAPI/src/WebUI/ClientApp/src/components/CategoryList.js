import React,{Component} from 'react';
import {get,remove} from '../helpers/apiHelpers';
import {Table} from 'react-bootstrap';
import {Button} from 'react-bootstrap';
import { CategoryFormModal } from './CategoryFormModal';

export class CategoryList extends Component{
    constructor(props){
        super(props);
        this.state={
            categories: [],
            categoryFormModalShow: false,
            categoryId: '',
            refresh: false
        };
    }
    
    async componentDidMount(){
        await this.handleGetCategories();
    }
    
    async handleGetCategories(){
        let categoryList = await get('categories');
        categoryList.sort((a, b) => (a.id - b.id));
        this.setState({categories: categoryList});
    }

    async handleDeleteCategory(id){
        await remove('categories/'+id);
        this.setState({refresh: true});
    }

    async componentDidUpdate(prevProps, prevState){
        if (!this.state.categoryFormModalShow){
            localStorage.removeItem('categoryId');
        }

        if(this.state.refresh !== prevState.refresh){
            await this.handleGetCategories();
            this.setState({
                refresh: false
              })
        }
    }

    render(){
        let categoryFormModalClose=()=>{
            this.setState({categoryFormModalShow: false});
            this.setState({refresh: true});
        }

        return(
            <div className="mt-4">
                <div id="category-nav">
                    <Button className="create-button" variant='success' onClick={()=>{this.setState({categoryFormModalShow: true})}}>
                        Create category
                    </Button>
                </div>
                <Table className="mt-4 category-table" striped bordered hover size="sm">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Name</th>
                            <th className="options-column">Options</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.categories.map((category, index) =>{
                            return(
                                <tr key={category.id}>
                                    <td>{index+1}</td>
                                    <td>{category.name}</td>
                                    <td>
                                    <Button className="update-button" variant='warning' onClick={()=>{
                                        localStorage.setItem('categoryId', category.id);
                                        this.setState({categoryId: category.id});
                                        this.setState({categoryFormModalShow: true})
                                        }}>
                                        Update
                                    </Button>
                                    <Button className="delete-button" variant='danger' onClick={()=>{
                                        this.handleDeleteCategory(category.id)
                                        }}>
                                        Delete
                                    </Button>
                                    </td>
                                </tr>
                            )
                        })
                        }
                    </tbody>
                </Table>
                <CategoryFormModal show={this.state.categoryFormModalShow} categoryid={this.state.categoryId} onHide={categoryFormModalClose}></CategoryFormModal>
            </div>
        )
    }
}