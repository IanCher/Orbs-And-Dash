import json
import bpy


def write_spline_data(context, filepath):
    print("running write_spline_data...")
    
    spline_data = [
        {
            "position": {
                "x": point.co.to_tuple()[0],
                "y": point.co.to_tuple()[2],
                "z": point.co.to_tuple()[1],
            },
            "tangentIn": {
                "x": point.handle_left.to_tuple()[0]-point.co.to_tuple()[0],
                "y": point.handle_left.to_tuple()[2]-point.co.to_tuple()[2],
                "z": point.handle_left.to_tuple()[1]-point.co.to_tuple()[1],
            },
            "tangentOut": {
                "x": point.handle_right.to_tuple()[0]-point.co.to_tuple()[0],
                "y": point.handle_right.to_tuple()[2]-point.co.to_tuple()[2],
                "z": point.handle_right.to_tuple()[1]-point.co.to_tuple()[1],
            },
        }
        for point_id, point in enumerate(bpy.data.curves[0].splines[0].bezier_points)
    ]
    
    spline = {"knots": spline_data}
    
    with open(filepath, "w", encoding='utf-8') as fid:
        json.dump(spline, fid)

    return {'FINISHED'}


# ExportHelper is a helper class, defines filename and
# invoke() function which calls the file selector.
from bpy_extras.io_utils import ExportHelper
from bpy.props import StringProperty, BoolProperty, EnumProperty
from bpy.types import Operator


class ExportSplineData(Operator, ExportHelper):
    """This appears in the tooltip of the operator and in the generated docs"""
    bl_idname = "export.spline"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Export Spline Data"

    # ExportHelper mix-in class uses this.
    filename_ext = ".json"

    def execute(self, context):
        return write_spline_data(context, self.filepath)


# Only needed if you want to add into a dynamic menu
def menu_func_export(self, context):
    self.layout.operator(ExportSplineData.bl_idname, text="Spline Export")


# Register and add to the "file selector" menu (required to use F3 search "Text Export Operator" for quick access).
def register():
    bpy.utils.register_class(ExportSplineData)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export)


def unregister():
    bpy.utils.unregister_class(ExportSplineData)
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export)


if __name__ == "__main__":
    register()

    # test call
#    bpy.ops.export_test.some_data('INVOKE_DEFAULT')
