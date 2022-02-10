import ProductRepo from './Repos/ProductsRepo';
import BrandRepo from './Repos/BrandRepo';
import CategoryRepo from './Repos/CategoryRepo';
import LeadsRepo from './Repos/LeadsRepo';


const repositories = {
    'products': ProductRepo,
    'brands': BrandRepo, 
    'categories': CategoryRepo,
    'leads':LeadsRepo,
}
export default {
    get: name => repositories[name]
};